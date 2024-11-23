using System.Text;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.Factor.Product;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using Kavenegar;
using Kavenegar.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

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

    public class MelatResult
    {
        public string RefId { get; set; }
        public string ResCode { get; set; }
        public string SaleOrderId { get; set; }
        public string SaleReferenceId { get; set; }
    }


    public async Task<string> ProductFactor(string discountCode, int postMethodId,
        int userAddressId, string desc, string economicNumber, string organizationName, string nationalId,
        string postCode, string organizationNumber, string registrationNumber, string adders, bool isLegal = false)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

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
                    if (i.Offer.StartDate.CalcOffer(i.Offer.Hours, i.Offer.Minutes, i.IsOffer) &&
                        i.ProductColors.Any(x => x.ColorId == i.Offer.ColorId))
                    {
                        offer += i.Offer.OfferAmount;
                        price += @i.Offer.StartDate.CalcOffer(i.DiscountAmount, i.Offer.OfferAmount,
                            i.ProductColors.FirstOrDefault(x => x.ColorId == i.Offer.ColorId).Price,
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

            var discountAmount = 0.0;
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
                DiscountCode = discountCode ?? string.Empty,
                UserAddressId = userAddressId == 0 ? null : userAddressId,
                DiscountAmount = discountAmount,
                PostMethodId = postMethodId,
                InsertDate = DateTime.Now,
                Status = Status.PendingForPayment,
                AmountPrice = offer,
                FactorCode = Helpers.CodeGenerator(user.Id, DateTime.Now.Month.ToString()),
                IsReturned = false,
                Desc = desc ?? string.Empty,
                AccountType = isLegal ? AccountType.Legal : AccountType.Genuine,
                EconomicNumber = economicNumber,
                IsLegal = isLegal,
                NationalId = nationalId,
                RegistrationNumber = registrationNumber ?? string.Empty,
                Adders = adders ?? string.Empty,
                OrganizationNumber = organizationNumber ?? string.Empty,
                OrganizationName = organizationName, PostCode = postCode
            };
            await _work.GenericRepository<Factor>().AddAsync(factor, CancellationToken.None);
            List<FactorProduct> fc = new List<FactorProduct>();
            List<FactorProductColor> ccf = new List<FactorProductColor>();
            foreach (var i in JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket"))
                         .ToList())
            {
                var prod = await _work.GenericRepository<ProductColor>().Table
                    .Include(x => x.Product).ThenInclude(x => x.Offer)
                    .Include(x => x.Product).ThenInclude(x => x.Brand)
                    .Include(x => x.Guarantee)
                    .Include(x => x.Color)
                    .FirstOrDefaultAsync(x => x.Id == i);
                int factorProdId = 0;
                if (!fc.Any(x => x.ProductId == prod.ProductId))
                {
                    var a = new FactorProduct
                    {
                        ProductId = prod.ProductId,
                        FactorId = factor.Id,
                        DiscountAmount = prod.Product.DiscountAmount,
                        UnicCode = prod.Product.UnicCode,
                        Title = prod.Product.Title,
                        PersianTitle = prod.Product.PersianTitle,
                        ImageUri = prod.Product.ImageUri,
                        Brand = prod.Product.Brand.Title,
                    };
                    await _work.GenericRepository<FactorProduct>().AddAsync(a, CancellationToken.None);
                    fc.Add(a);
                    factorProdId = a.Id;
                }
                else
                {
                    var ffc = fc.FirstOrDefault(x => x.ProductId == prod.ProductId);
                    factorProdId = ffc.Id;
                }

                if (!ccf.Any(x => x.ColorId == prod.ColorId))
                {
                    var c = new FactorProductColor
                    {
                        Price = prod.Price,
                        Count = 1,
                        ColorId = prod.ColorId,
                        Guarantee = prod.Guarantee.Title,
                        ColorCode = prod.Color.ColorCode,
                        FactorProductId = factorProdId,
                        ColorName = prod.Color.Title
                    };
                    if (prod.Product.IsOffer)
                    {
                        if (prod.Product.Offer.StartDate.CalcOffer(prod.Product.Offer.Hours, prod.Product.Offer.Minutes,
                                prod.Product.IsOffer) && prod.Product.Offer.ColorId == prod.ColorId)
                        {
                            c.OfferAmount = prod.Product.Offer.OfferAmount;
                        }
                    }

                    ccf.Add(c); // Add the new item to ccf
                }
                else
                {
                    var q = ccf.FirstOrDefault(x => x.ColorId == prod.ColorId);
                    if (q != null)
                    {
                        q.Count++; // Increment the count for the existing color
                    }
                }

                prod.Inventory--;
                await _work.GenericRepository<ProductColor>().UpdateAsync(prod, CancellationToken.None);
            }

            await _work.GenericRepository<FactorProductColor>().AddRangeAsync(ccf, CancellationToken.None);


            HttpClient httpClient = new HttpClient();

            var result =
                await httpClient.GetAsync(
                    $"https://front.parsme.com/bank/Index?Price={factor.Amount}&factorId={factor.Id}&userId={user.Id}");
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(response))
                {
                    KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
                    webApi.VerifyLookup(user.PhoneNumber, user.Name, factor.FactorCode, "-", "ProcessingOrder");
                    ViewBag.RefId = response;
                    ViewBag.UrlBank = BpmConfig.PostUrl;
                    ViewBag.Error = false;
                    ViewBag.Error = "در حال اتصال به در گاه بانک به پرداخت ملت";
                    return response;
                }
                else
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    public async Task<IActionResult> MellatBack(string RefId, string ResCode, string SaleOrderId,
        string SaleReferenceId, string orderid)
    {
        ViewBag.Message = null;
        ViewBag.IsSuccess = false;
        ViewBag.RefId = RefId;
        ViewBag.ResCode = ResCode;
        ViewBag.SaleReferenceId = SaleReferenceId;
        //Order Id Back
        ViewBag.SaleOrderId = SaleOrderId;

        try
        {
            if (ResCode == "0")
            {
                MelatResult melatResult = new MelatResult()
                {
                    RefId = RefId,
                    ResCode = ResCode,
                    SaleOrderId = SaleOrderId,
                    SaleReferenceId = SaleReferenceId
                };

                HttpClient httpClient = new HttpClient();
                var apiSerlize = JsonConvert.SerializeObject(melatResult);
                var content = new StringContent(apiSerlize, Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync($"https://front.parsme.com/Bank/MellatResponse", content);

                if (result.IsSuccessStatusCode)
                {
                    var _value = await result.Content.ReadAsStringAsync();
                    ApiResult.MelatResultResponse melatMessage =
                        JsonConvert.DeserializeObject<ApiResult.MelatResultResponse>(_value);

                    if (melatMessage.data.isError)
                    {
                        ViewBag.Message = melatMessage.data.errorMsg;
                    }
                    else
                    {
                        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
                        ViewBag.Message = melatMessage.data.succeedMsg;
                        ViewBag.IsSuccess = true;
                        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(new List<int>()));
                    }
                }
            }
            else
            {
                ViewBag.Message = "پرداخت آنلاین با خطا مواجه شده است.";
            }
        }
        catch
        {
            ViewBag.Message = "خطا در برقراری ارتباط با درگاه بانک ملت";
        }

        ViewBag.Factor = await _work.GenericRepository<Factor>().TableNoTracking
            .Include(x => x.PostMethod)
            .Include(x => x.UserAddress)
            .Include(x => x.Products).ThenInclude(x => x.FactorProductColor)
            .FirstOrDefaultAsync(x => x.Id == SaleOrderId.ToInt());
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

        ViewBag.BasketProd = basketProducts;
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }
}