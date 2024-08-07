using Application.Common.Utilities;
using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp.Controllers;

public class PaymentController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _work;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    // GET
    public PaymentController(IMediator mediator, ILogger<HomeController> logger, IUnitOfWork work,
        RoleManager<Role> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _mediator = mediator;
        _logger = logger;
        _work = work;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ProductFactor(string discountCode, int postMethodId,
        int userAddressId, string desc)
    {
        if (User.Identity.IsAuthenticated)
        {
           
            var user =await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
           
            var basketProducts = new List<Product>();
            if (HttpContext.Session.GetString("basket") != null)
            {
                var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket"))
                    .ToList();
                foreach (var i in basketList)
                {
                    var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x => x.Color)
                        .FirstOrDefaultAsync(x => x.Id == i);

                    var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                        .ThenInclude(x => x.Color)
                        .Include(x => x.Offer)
                        .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                    prod.ProductColors = new List<ProductColor>() { prodColor };
                    basketProducts.Add(prod!);
                }
            }

            var prods = basketProducts;
            double price = 0;
            double offer = 0;
            foreach (var i in prods)
            {
                if (i.IsOffer)
                {
                    if (i.Offer.StartDate.CalcOffer(i.Offer.Days, i.Offer.Hours, i.Offer.Minutes, i.IsOffer) &&
                        i.ProductColors.Any(x => x.ColorId == i.Offer.ColorId))
                    {
                        offer += i.Offer.OfferAmount;
                        price += @i.Offer.StartDate.CalcOffer(i.DiscountAmount, i.Offer.OfferAmount,
                            i.ProductColors.FirstOrDefault(x => x.ColorId == i.Offer.ColorId).Price, i.Offer.Days,
                            i.Offer.Hours, i.Offer.Minutes, i.IsOffer);
                    }
                    else
                    {
                        offer += i.DiscountAmount;
                        price += @i.ProductColors.FirstOrDefault()!.Price.DiscountProduct(i.DiscountAmount);
                    }
                }
                else
                {
                    offer += i.DiscountAmount;
                    price += @i.ProductColors.FirstOrDefault()!.Price.DiscountProduct(i.DiscountAmount);
                }
            }
            var discountAmount =  0.0;
            if (!string.IsNullOrWhiteSpace(discountCode))
            {
                var discount = await _work.GenericRepository<DiscountCode>().Table
                    .FirstOrDefaultAsync(x => x.Code == discountCode);
                discountAmount = discount.Amount;
                discount.Count--;
                await _work.GenericRepository<DiscountCode>().UpdateAsync(discount, CancellationToken.None);
            }
            var factor = new Factor()
            {
                Amount = price,
                UserId = user.Id,
                DiscountCode = discountCode,
                UserAddressId = userAddressId,
                DiscountAmount = discountAmount,
                PostMethodId = postMethodId,
                InsertDate = DateTime.Now,
                Status = Status.Pending,
                AmountPrice = offer,
                FactorCode = Helpers.CodeGenerator(user.Id, DateTime.Now.Month.ToString()),
                IsReturned = false,
                Desc = desc,
            };
            await _work.GenericRepository<Factor>().AddAsync(factor, CancellationToken.None);
           
            foreach (var i in JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket"))
                         .ToList())
            {
                var prod = await _work.GenericRepository<ProductColor>().Table
                    .FirstOrDefaultAsync(x => x.Id == i);
                await _work.GenericRepository<FactorProduct>().AddAsync(new FactorProduct
                {
                    ProductColorId = prod.Id,
                    FactorId = factor.Id
                }, CancellationToken.None);
            }

            ViewBag.Factor = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x=>x.PostMethod)
                .Include(x=>x.UserAddress)
                .Include(x=>x.Products).ThenInclude(x=>x.ProductColor).ThenInclude(x=>x.Product)
                .Include(x => x.Products)     .Include(x => x.Products)
                .ThenInclude(x => x.ProductColor).ThenInclude(x => x!.Product)
                .FirstOrDefaultAsync(x => x.Id == factor.Id);
            ViewBag.BasketProd = basketProducts;
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    
    

}