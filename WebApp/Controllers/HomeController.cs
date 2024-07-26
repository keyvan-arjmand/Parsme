using System.Diagnostics;
using Application.Admin.V1.Commands.ConfirmCodAdmin;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Interfaces;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Domain.Entity.User;
using Domain.Enums;
using Kavenegar;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _work;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork work, SignInManager<User> signInManager,
        RoleManager<Role> roleManager, UserManager<User> userManager, IMediator mediator)
    {
        _logger = logger;
        _work = work;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var cats = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            foreach (var id in basketList)
            {
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x => x.Color)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.Offer)
                    .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.BasketProd = basketProducts;

        ViewBag.ProdCatalog = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
            .Include(x=>x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Take(20).ToListAsync();
        var otherProd = new List<Product>();
        foreach (var i in cats.Select(x => x.Id).ToList())
        {
            otherProd.AddRange(await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
                .Include(x=>x.Offer)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Take(4).ToListAsync());
        }

        ViewBag.OtherParsme = otherProd;
        ViewBag.BestSeller = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
            .Include(x=>x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Take(10).ToListAsync();
        ViewBag.Banners = await _work.GenericRepository<Banner>().TableNoTracking.FirstOrDefaultAsync() ?? new Banner();
        ViewBag.NewProd = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory)
            .Include(x=>x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .OrderBy(x => x.InsertDate).Take(20).ToListAsync();
        ViewBag.OfferMoments =
            await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Offer)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Where(x => x.MomentaryOffer).Take(7).ToListAsync();


        var offer = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Include(x => x.Offer)
            .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail).Where(x => x.IsOffer).Take(7)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        // foreach (var i in offer.ProductDetails)
        // {
        //     i.CategoryDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
        //         .FirstOrDefaultAsync(x => x.Id == i.CategoryDetailId);
        // }

        ViewBag.Offer = offer;

        return View();
    }

    public class UserLoginDto
    {
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public async Task<ActionResult> UserLogin(string password, string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
            if (!result.Succeeded)
                return RedirectToAction("login", "Home");
            return RedirectToAction("Profile", "Home");
        }
        else
        {
            return RedirectToAction("login", "Home");
        }
    }

    public async Task<ActionResult> AddToFav(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                await _work.GenericRepository<UserFav>().AddAsync(new UserFav
                {
                    ProductId = id,
                    UserId = user.Id
                }, CancellationToken.None);
            }

            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("login", "Home");
        }
    }

    public async Task<IActionResult> Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.User = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                           new User();
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


            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> UserAddress()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            ViewBag.User = user;
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
            ViewBag.Address = await _work.GenericRepository<UserAddress>().TableNoTracking.Include(x => x.City)
                .ThenInclude(x => x.State)
                .Where(x => x.UserId == user.Id).ToListAsync();
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> UserAddAddress()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            ViewBag.User = user;
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

            ViewBag.City = await _work.GenericRepository<City>().TableNoTracking.ToListAsync();
            ViewBag.BasketProd = basketProducts;
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

            return View("AddAddress");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> UserFav()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            ViewBag.User = user;
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

            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

            ViewBag.BasketProd = basketProducts;
            ViewBag.Fav = await _work.GenericRepository<UserFav>().TableNoTracking
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Product).ThenInclude(x => x.Offer).ThenInclude(x => x.Color)
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> OrderDetail()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.User = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                           new User();
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


            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> UserOrder()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.User = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                           new User();
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


            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> EditeProfile(string name, string family, string oldPassword, string newPassword,
        string email)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            if (user != null)
            {
                user.Name = name;
                user.Family = family;
                user.Email = email;

                if (!string.IsNullOrWhiteSpace(oldPassword))
                {
                    if (user.Password == oldPassword)
                    {
                        user.Password = newPassword;
                    }

                    await _userManager.UpdateAsync(user);
                    await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                }
                else
                {
                    await _userManager.UpdateAsync(user);
                }
            }

            return RedirectToAction("ProfileDetail");
        }
        else
        {
            throw new Exception();
        }
    }

    public async Task<IActionResult> InsertUserAddress(string name, string address, int cityId, string number,
        string postCode)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            await _work.GenericRepository<UserAddress>().AddAsync(new UserAddress
            {
                Address = address,
                CityId = cityId,
                PostCode = postCode,
                Name = name,
                Number = number, UserId = user.Id
            }, CancellationToken.None);

            return RedirectToAction("ProfileDetail");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> ProductPage(int id)
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.Product = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.Offer)
            .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail)
            .Include(x => x.ProductImages)
            .Include(x => x.SubCategory).ThenInclude(x => x.Category)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
            .FirstOrDefaultAsync(x => x.Id == id) ?? new Product();

        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        ViewBag.SaleService = await _work.GenericRepository<SaleServices>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SaleServices();
        return View();
    }

    public async Task<IActionResult> GetByBrand(int id)
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.Offer)
            .Where(x => x.BrandId == id).ToListAsync();


        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        return View("ProductByBrand");
    }

    public async Task<IActionResult> Faq()
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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

        ViewBag.Faq = await _work.GenericRepository<Faq>().Table.ToListAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }
    public async Task<IActionResult> AboutUs()
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        return View();
    }
    public async Task<IActionResult> ContactUs()
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        return View();
    }
    public async Task<IActionResult> Search(string search, double min, double max)
    {
        ViewBag.Search = search;
        if (max > 0)
        {
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.SubCategory)
                .ThenInclude(sc => sc.Category)
                .Include(x => x.Offer)
                .Where(x => x.SubCategory.Name.Contains(search) ||
                            x.SubCategory.Category.Name.Contains(search) ||
                            x.PersianTitle.Contains(search) ||
                            x.Title.Contains(search) ||
                            x.Brand.Title.Contains(search) ||
                            x.Detail.Contains(search))
                .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                .ToListAsync();
        }
        else
        {
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.ProductColors)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Offer)
                .Where(x => x.SubCategory.Name.Contains(search) || x.SubCategory.Category.Name.Contains(search) ||
                            x.PersianTitle.Contains(search) || x.Title.Contains(search) ||
                            x.Brand.Title.Contains(search) ||
                            x.Detail.Contains(search)).ToListAsync();
        }

        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Brands)
            .ToListAsync();
        ViewBag.Brand = await _work.GenericRepository<Brand>().TableNoTracking.Take(7).ToListAsync();
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        return View();
    }

    public async Task<IActionResult> SubCategory(int subCategoryId, double min, double max, int detailId, string value)
    {
        ViewBag.Id = subCategoryId;
        if (max > 0)
        {
            if (detailId > 0)
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.ProductDetails)
                    .Include(x => x.Offer)
                    .Where(x => x.SubCategoryId == subCategoryId)
                    .Where(x =>
                        x.ProductDetails.FirstOrDefault(q => q.CategoryDetailId == detailId)!.Value.Contains(value))
                    .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                    .ToListAsync();
            }
            else
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.Offer)
                    .Where(x => x.SubCategoryId == subCategoryId)
                    .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                    .ToListAsync();
            }
        }
        else
        {
            if (detailId > 0)
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.Offer)
                    .Include(x => x.ProductDetails)
                    .Where(x => x.SubCategoryId == subCategoryId)
                    .Where(x =>
                        x.ProductDetails.FirstOrDefault(q => q.CategoryDetailId == detailId)!.Value.Contains(value))
                    .ToListAsync();
            }
            else
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.Offer)
                    .Where(x => x.SubCategoryId == subCategoryId)
                    .ToListAsync();
            }
        }

        ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
            .Where(x => x.SubCategoryId == subCategoryId && x.ShowInSearch).ToListAsync();
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Brands)
            .ToListAsync();
        ViewBag.Brand = await _work.GenericRepository<Brand>().TableNoTracking
            .Where(x => x.SubCategoryId == subCategoryId).ToListAsync();
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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
        return View();
    }

    public async Task<IActionResult> AddToBasket(int id)
    {
        var basketlist = new List<int>();

        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            basketlist.Add(id);
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        }
        else
        {
            basketlist.Add(id);
            HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public async Task<List<Product>> GetProdComparison(int subCatId,string search)
    {
        var prods = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Where(x=>x.SubCategoryId==subCatId)
            .Where(x => 
                        x.PersianTitle.Contains(search) ||
                        x.Title.Contains(search) ||
                        x.Detail.Contains(search)).ToListAsync();
        return prods;
    }

    public async Task<IActionResult> ComparisonProduct(int id)
    {
        var comparison = new List<int>();

        if (HttpContext.Session.GetString("comparison") != null)
        {
            comparison = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("comparison")).ToList();
            if (id > 0)
            {
                if (comparison.Count() > 0)
                {
                    var pp = await _work.GenericRepository<Product>().TableNoTracking
                        .FirstOrDefaultAsync(x => x.Id == comparison.First());
                    var a = await _work.GenericRepository<Product>().TableNoTracking
                        .FirstOrDefaultAsync(x => x.Id == id);
                    if (a.SubCategoryId == pp.SubCategoryId)
                    {
                        comparison.Add(id);
                        HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
                    }
                }
                else
                {
                    comparison.Add(id);
                    HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
                }
            }
        }
        else
        {
            if (id > 0)
            {
                comparison.Add(id);
                HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
            }
        }


        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        var prods = new List<Product>();
        var ProdBySubCat = new List<Product>();
        var CatDetails = new List<CategoryDetail>();
        if (comparison.Count() > 0)
        {
            prods = await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.Offer)
                .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail)
                .Include(x => x.ProductImages)
                .Include(x => x.SubCategory).ThenInclude(x => x.Category)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
                .Where(x => comparison.Contains(x.Id)).ToListAsync();


            CatDetails = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                .Where(x => x.SubCategoryId == prods.FirstOrDefault().SubCategoryId).ToListAsync();
            ProdBySubCat = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Offer)
                .Where(x => x.SubCategoryId == prods.FirstOrDefault().SubCategoryId).Take(10).ToListAsync();
        }

        ViewBag.Prods = prods;
        ViewBag.ProdBySubCat = ProdBySubCat;
        ViewBag.CatDetails = CatDetails;

        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
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

        return View();
    }

    public async Task<IActionResult> SignUp(string phoneNumber, string pass)
    {
        if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber))
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            if (user.Password == pass)
            {
                await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
                return RedirectToAction("Profile");
            }

            return RedirectToAction("Login");
        }
        else
        {
            var user = new Domain.Entity.User.User
            {
                Family = string.Empty,
                Name = string.Empty,
                PhoneNumber = phoneNumber,
                Email = string.Empty,
                Password = pass,
                InsertDate = DateTime.Now,
                UserName = phoneNumber,
                SecurityStamp = string.Empty,
                CityId = 1,
                Sheba = string.Empty,
                NationalCode = string.Empty,
            };
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "user"
                });
            }

            await _userManager.CreateAsync(user, pass);

            await _userManager.AddToRoleAsync(user, "user");
            await _userManager.UpdateAsync(user);
            return RedirectToAction("ConfirmPhoneNumber", new { user.PhoneNumber });
        }
    }

    public async Task<IActionResult> Basket()
    {
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            foreach (var i in basketList)
            {
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x => x.Color)
                    .Include(x => x.Guarantee)
                    .FirstOrDefaultAsync(x => x.Id == i);

                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.Offer)
                    .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.Address = await _work.GenericRepository<UserAddress>().TableNoTracking
            .ToListAsync();
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.BasketProd = basketProducts;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult RemoveInComparison(int id)
    {
        var basketlist = new List<int>();
        if (HttpContext.Session.GetString("comparison") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("comparison")).ToList();
            basketlist.Remove(id);
            HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(basketlist));
        }

        return RedirectToAction("ComparisonProduct", "Home", new { id = 0 });
    }

    public IActionResult RemoveInBasket(int id)
    {
        var basketlist = new List<int>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
        }

        basketlist.Remove(id);
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> Login()
    {
        return View();
    }

    public async Task<ActionResult> LoginPhone()
    {
        return View();
    }

    public async Task<ActionResult> ConfirmPhoneNumber(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(3);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(phoneNumber, user.ConfirmCode,
            "VerifyCodeFaani");
        @ViewBag.Phone = phoneNumber;
        return View();
    }

    public async Task<ActionResult> Register()
    {
        return View();
    }

    public async Task<ActionResult> ConfirmCode(string phoneNumber)
    {
        ViewBag.Phone = phoneNumber;
        return View();
    }

    public async Task<ActionResult> WelcomeParsme()
    {
        return View();
    }

    public async Task<ActionResult> ValidateCode(string phoneNumber, string code)
    {
        var user = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
        await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
        return Ok();
    }

    public async Task<ActionResult> ValidateCodeUser(string phoneNumber, string code)
    {
        var user = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
        await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
        return Ok();
    }

    public async Task<ActionResult> SendLoginCode(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(3);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(phoneNumber, user.ConfirmCode,
            "VerifyCodeFaani");
        return Ok(
        );
    }

    public async Task<ActionResult> ProfileDetail()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
                .Include(x => x.SubCategories)
                .ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.User = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                           new User();
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

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();

            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> UserRegister(string number, string password)
    {
        if (!await _userManager.Users.AnyAsync(x => x.PhoneNumber == number))
        {
            var user = new Domain.Entity.User.User
            {
                Family = "",
                Name = "user",
                PhoneNumber = number,
                Email = string.Empty,
                Password = password,
                InsertDate = DateTime.Now,
                UserName = number,
                SecurityStamp = string.Empty,
                CityId = 1,
            };
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "user"
                });
            }

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "user");
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Login");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        else return RedirectToAction("Index", "Home");
    }
}