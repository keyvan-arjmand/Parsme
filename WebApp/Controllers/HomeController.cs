using System.Diagnostics;
using Application.Admin.V1.Commands.ConfirmCodAdmin;
using Application.Common.Utilities;
using Application.Constants.Kavenegar;
using Application.Dtos;
using Application.Dtos.Products;
using Application.Interfaces;
using Domain.Entity.Factor;
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
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .AsSplitQuery()
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.BasketProd = basketProducts;

        ViewBag.ProdCatalog = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .AsSplitQuery()
            .Where(x => x.IsShowIndex)
            .Take(20).ToListAsync();
        var otherProd = new List<Product>();
        foreach (var i in cats.Select(x => x.Id).ToList())
        {
            otherProd.AddRange(await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.SubCategory)
                .Include(x => x.Offer)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .AsSplitQuery()
                .Take(4).ToListAsync());
        }

        ViewBag.OtherParsme = otherProd;
        ViewBag.BestSeller = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .AsSplitQuery()
            .OrderByDescending(x => x.InterestRate)
            .Take(10).ToListAsync();
        ViewBag.Banners = await _work.GenericRepository<Banner>().TableNoTracking.FirstOrDefaultAsync() ?? new Banner();
        ViewBag.NewProd = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .AsSplitQuery()
            .OrderByDescending(x => x.InsertDate).Take(20).ToListAsync();
        ViewBag.OfferMoments =
            await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Offer)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .AsSplitQuery()
                .Where(x => x.MomentaryOffer).Take(7).ToListAsync();


        var offer = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Include(x => x.Offer)
            .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail)
            .AsSplitQuery()
            .Where(x => x.IsOffer && x.IsActive && !x.IsDelete)
            .Take(7)
            .ToListAsync();
        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        // foreach (var i in offer.ProductDetails)
        // {
        //     i.CategoryDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
        //         .FirstOrDefaultAsync(x => x.Id == i.CategoryDetailId);
        // }

        ViewBag.Offer = offer;

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    public class UserLoginDto
    {
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public async Task<ActionResult> UserLogin(string? password, string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
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

            var previousUrl = Request.Headers["Referer"];

            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            return RedirectToAction("login", "Home");
        }
    }

    public async Task<ActionResult> RemoveInFav(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var fav = await _work.GenericRepository<UserFav>().TableNoTracking
                    .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id);
                await _work.GenericRepository<UserFav>().DeleteAsync(fav, CancellationToken.None);
            }

            var previousUrl = Request.Headers["Referer"];

            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            else
            {
                return RedirectToAction("UserFav", "Home");
            }
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
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();

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
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> EditAddress(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id);
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
            ViewBag.City = await _work.GenericRepository<City>().TableNoTracking.ToListAsync();
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
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
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
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
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> OrderDetail(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.Order = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x => x.User)
                .Include(x => x.PostMethod)
                .Include(x => x.UserAddress)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor).ThenInclude(x => x!.FactorProduct)
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.ReturnFactor = await _work.GenericRepository<ReturnedFactor>().TableNoTracking
                .FirstOrDefaultAsync(x => x.FactorId == id);
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
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
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.Order = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x => x.User)
                .Include(x => x.PostMethod)
                .Include(x => x.UserAddress)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor).ThenInclude(x => x!.FactorProduct)
                .Where(x => x.User.UserName == User.Identity.Name).ToListAsync();
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> EditeProfile(string name, string family,
        string email, string Sheba, string NationalCode)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
            if (!NationalCode.IsValidIranianNationalCode()) throw new Exception();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            if (user != null)
            {
                user.Name = name;
                user.Family = family;
                user.Email = email;
                user.Sheba = Sheba;
                user.NationalCode = NationalCode;

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("ProfileDetail");
        }
        else
        {
            throw new Exception();
        }
    }

    public async Task CastLanding()
    {
        var brand = await _work.GenericRepository<BrandLanding>().TableNoTracking
            .FirstOrDefaultAsync(x => x.BrandTagId == 8);
        var brands = await _work.GenericRepository<BrandTag>().TableNoTracking.Where(x => x.Id != 9)
            .ToListAsync();
        foreach (var i in brands)
        {
            await _work.GenericRepository<BrandLanding>().AddAsync(new BrandLanding
            {
                SmallBanner1 = brand.SmallBanner1,
                BrandTagId = i.Id,
                BigBanner = brand.BigBanner,
                DescSlider = brand.DescSlider,
                DescSlider2 = brand.DescSlider2,
                DescSlider3 = brand.DescSlider3,
                DescSlider4 = brand.DescSlider4,
                DescSlider5 = brand.DescSlider5,
                HrefSlider = brand.HrefSlider,
                HrefSlider2 = brand.HrefSlider2,
                HrefSlider3 = brand.HrefSlider3,
                HrefSlider4 = brand.HrefSlider4,
                HrefSlider5 = brand.HrefSlider5,
                ImageSlider = brand.ImageSlider,
                ImageSlider5 = brand.ImageSlider5,
                ImageSlider2 = brand.ImageSlider2,
                SmallBanner2 = brand.SmallBanner2,
                SmallBanner3 = brand.SmallBanner3,
                SmallBanner4 = brand.SmallBanner4,
                ImageSlider3 = brand.ImageSlider3,
                ImageSlider4 = brand.ImageSlider4,
                TitleSlider3 = brand.TitleSlider3,
                TitleSlider = brand.TitleSlider,
                TitleSlider2 = brand.TitleSlider2,
                TitleSlider4 = brand.TitleSlider4,
                HrefSmallBanner1 = brand.HrefSmallBanner1,
                TitleSlider5 = brand.TitleSlider5,
                HrefSmallBanner4 = brand.HrefSmallBanner4,
                HrefBigBanner = brand.HrefBigBanner,
                HrefSmallBanner2 = brand.HrefSmallBanner2,
                HrefSmallBanner3 = brand.HrefSmallBanner3
            }, CancellationToken.None);
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

            return RedirectToAction("UserAddress");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> ReturnedFactor(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.Order = await _work.GenericRepository<Factor>().TableNoTracking
                .Include(x => x.User)
                .Include(x => x.PostMethod)
                .Include(x => x.UserAddress)
                .Include(x => x.Products)
                .ThenInclude(x => x.FactorProductColor).ThenInclude(x => x!.FactorProduct)
                .FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> InsertReturnedFactor(int id, string Desc, int Type)
    {
        if (User.Identity.IsAuthenticated)
        {
            var factor = await _work.GenericRepository<Factor>().Table.FirstOrDefaultAsync(x => x.Id == id);
            factor.IsReturned = true;
            await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
            await _work.GenericRepository<ReturnedFactor>().AddAsync(new ReturnedFactor
            {
                ReturnedType = (ReturnedType)Type,
                InsertDate = DateTime.Now,
                FactorId = id,
                Desc = Desc
            }, CancellationToken.None);
            return RedirectToAction("UserOrder");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> UpdateAddress(int id, string name, string address, int cityId, string number,
        string postCode)
    {
        if (User.Identity.IsAuthenticated)
        {
            var result = await _work.GenericRepository<UserAddress>().Table.FirstOrDefaultAsync(x => x.Id == id);
            result.Name = name;
            result.Address = address;
            result.CityId = cityId;
            result.Number = number;
            result.PostCode = postCode;
            await _work.GenericRepository<UserAddress>().UpdateAsync(result, CancellationToken.None);
            return RedirectToAction("UserAddress");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> RemoveAddress(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            var address = await _work.GenericRepository<UserAddress>().Table
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id);
            address.IsDelete = true;
            await _work.GenericRepository<UserAddress>().UpdateAsync(address, CancellationToken.None);
            return RedirectToAction("UserAddress");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Checkout(string code, int postMethod)
    {
        if (User.Identity.IsAuthenticated)
        {
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

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            ViewBag.Code = await _work.GenericRepository<DiscountCode>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Code == code) ?? new DiscountCode();
            ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Id == postMethod);
            ViewBag.Address = await _work.GenericRepository<UserAddress>().TableNoTracking.ToListAsync();
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> DiscountCheck(string code)
    {
        var result = _work.GenericRepository<DiscountCode>().TableNoTracking
            .Any(x => x.Code == code && x.IsActive && x.Count > 0);
        if (result)
        {
            return Ok();
        }
        else
        {
            throw new Exception();
            ;
        }
    }


    public async Task<IActionResult> InsertContact(string Name, string PhoneNumber, string Email, string Message,
        int Subject)
    {
        await _work.GenericRepository<ContactUs>().AddAsync(new ContactUs()
        {
            Email = Email,
            PhoneNumber = PhoneNumber,
            Name = Name,
            Message = Message,
            Subject = (Subject)Subject,
            InsertDate = DateTime.Now
        }, CancellationToken.None);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ProductPage(int id)
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        var prodD = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.Offer)
            .Include(x => x.BrandTag)
            .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail).ThenInclude(q => q.Feature)
            .Include(x => x.ProductImages)
            .Include(x => x.SubCategory).ThenInclude(x => x.Category).ThenInclude(x => x!.MainCategory)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.ProductColors).ThenInclude(x => x.Guarantee)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
        if (prodD == null) throw new Exception();

        prodD.ProductDetails = prodD.ProductDetails.OrderByDescending(x => x.CategoryDetail.Priority).ToList();
        ViewBag.Product = prodD;
        var prods = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
            .Include(x => x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Where(x => x.SubCategoryId == prodD.SubCategoryId && x.Id != prodD.Id)
            .Take(12).ToListAsync();
        if (prods.Count <= 0)
        {
            ViewBag.Prods = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
                .Include(x => x.Offer)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Where(x => x.Id != prodD.Id)
                .Take(12).ToListAsync();
        }
        else
        {
            ViewBag.Prods = prods;
        }

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
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    public async Task<IActionResult> GetByBrand(int id, double min, double max, int page = 1)
    {
        ViewBag.Id = id;
        ViewBag.Page = page;
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        if (max > 0)
        {
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Offer)
                .Where(x => x.BrandId == id)
                .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                .Skip((page - 1) * 10)
                .Take(12)
                .ToListAsync();
        }
        else
        {
            ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Offer)
                .Where(x => x.BrandId == id)
                .Skip((page - 1) * 10)
                .Take(12)
                .ToListAsync();
        }


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

        ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Brands)
            .ToListAsync();
        ViewBag.Landing = await _work.GenericRepository<BrandLanding>().TableNoTracking
            .FirstOrDefaultAsync(x => x.BrandTagId == id) ?? new();
        ViewBag.BasketProd = basketProducts;
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View("ProductByBrand");
    }


    public async Task<IActionResult> Faq()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    public async Task<IActionResult> InstallmentPage()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    public async Task<IActionResult> AboutUs()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.BasketProd = basketProducts;
        ViewBag.About = await _work.GenericRepository<AboutUsPage>().TableNoTracking.FirstOrDefaultAsync();
        return View();
    }

    public async Task<IActionResult> ContactUs()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.BasketProd = basketProducts;
        ViewBag.Contact = await _work.GenericRepository<ContactPage>().TableNoTracking.FirstOrDefaultAsync();
        return View();
    }

    public async Task<IActionResult> ShowAll(string q, int page = 1)
    {
        ViewBag.q = q;
        ViewBag.Page = page;

        ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Include(x => x.SubCategory)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.Offer)
            .Skip((page - 1) * 10)
            .Take(12)
            .ToListAsync();

        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> Search(string search, double min, double max, int page = 1)
    {
        ViewBag.SearchR = search;
        ViewBag.Page = page;
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
                            x.Detail.Contains(search) || x.MetaKeyword.Contains(search))
                .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                .Skip((page - 1) * 10)
                .Take(12)
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
                            x.Detail.Contains(search) || x.MetaKeyword.Contains(search))
                .Skip((page - 1) * 10)
                .Take(12)
                .ToListAsync();
        }

        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> SubCategory(int id, double min, double max, List<string> values,
        int page = 1)
    {
        ViewBag.Id = id;
        ViewBag.Page = page;
        ViewBag.Values = values;
        if (max > 0)
        {
            if (values.Count > 0)
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.Offer)
                    .AsSplitQuery()
                    .OrderBy(x => x.Id)
                    .Where(x => x.SubCategoryId == id)
                    .Where(x => x.ProductDetails.Any(q => values.Contains(q.Value)))
                    .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                    .Skip((page - 1) * 10)
                    .Take(12)
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
                    .AsSplitQuery()
                    .OrderBy(x => x.Id)
                    .Where(x => x.SubCategoryId == id)
                    .Where(x => x.ProductColors.Any(c => c.Price >= min && c.Price <= max))
                    .Skip((page - 1) * 10)
                    .Take(12)
                    .ToListAsync();
            }
        }
        else
        {
            if (values.Count > 0)
            {
                ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x => x.SubCategory)
                    .ThenInclude(sc => sc.Category)
                    .Include(x => x.Offer)
                    .Include(x => x.ProductDetails)
                    .AsSplitQuery()
                    .OrderBy(x => x.Id)
                    .Where(x => x.SubCategoryId == id)
                    .Where(x => x.ProductDetails.Any(q => values.Contains(q.Value)))
                    .Skip((page - 1) * 10)
                    .Take(12)
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
                    .Include(x => x.ProductDetails)
                    .AsSplitQuery()
                    .OrderBy(x => x.Id)
                    .Where(x => x.SubCategoryId == id)
                    .Skip((page - 1) * 10)
                    .Take(12)
                    .ToListAsync();
            }
        }

        ViewBag.CatDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
            .Include(x => x.Feature)
            .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == id) &&
                        x.ShowInSearch).ToListAsync();

        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();

        ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Brands)
            .ToListAsync();
        ViewBag.Brand = await _work.GenericRepository<Brand>().TableNoTracking
            .Where(x => x.SubCategoryId == id).ToListAsync();
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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
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

        var previousUrl = Request.Headers["Referer"];

        if (!string.IsNullOrEmpty(previousUrl))
        {
            return Redirect(previousUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Privacy()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> GetProdComparison(int subCatId, string search)
    {
        var prods = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Where(x => x.SubCategoryId == subCatId)
            .Where(x => x.SubCategory.Name.Contains(search) ||
                        x.SubCategory.Category.Name.Contains(search) ||
                        x.PersianTitle.Contains(search) ||
                        x.Title.Contains(search) ||
                        x.Brand.Title.Contains(search) ||
                        x.Detail.Contains(search) || x.MetaKeyword.Contains(search)).Select(x => new ProductDto
            {
                ProductColors = x.ProductColors,
                Id = x.Id,
                PersianTitle = x.PersianTitle,
                ImageUri = x.ImageUri
            }).ToListAsync();
        return Ok(prods);
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
                        .AsQueryable()
                        .FirstOrDefaultAsync(x => x.Id == comparison.First());
                    var a = await _work.GenericRepository<Product>().TableNoTracking
                        .AsQueryable()
                        .FirstOrDefaultAsync(x => x.Id == id);
                    if (a.SubCategoryId == pp.SubCategoryId)
                    {
                        if (comparison.All(x => x != id))
                        {
                            comparison.Add(id);
                            HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(new List<int>(id)));
                    }
                }
                else
                {
                    if (comparison.All(x => x != id))
                    {
                        comparison.Add(id);
                        HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
                    }
                }
            }
        }
        else
        {
            if (id > 0)
            {
                if (comparison.All(x => x != id))
                {
                    comparison.Add(id);
                    HttpContext.Session.SetString("comparison", JsonConvert.SerializeObject(comparison));
                }
            }
        }

        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .AsSplitQuery()
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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
                .AsSplitQuery()
                .Where(x => comparison.Contains(x.Id)).ToListAsync();


            CatDetails = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                .Include(x => x.Feature)
                .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == prods.FirstOrDefault().SubCategoryId))
                .ToListAsync();

            ProdBySubCat = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                .Include(x => x.SubCategory)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.Offer)
                .AsSplitQuery()
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
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == prodColor.ProductId);

                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.BasketProd = basketProducts;
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    public async Task<IActionResult> SignUp(string phoneNumber)
    {
        if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber) ||
            !phoneNumber.IsValidIranianPhoneNumber())
        {
            return RedirectToAction("Login");
        }
        else
        {
            string pass = Guid.NewGuid().ToString().Substring(0, 9) + "!";
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

    public async Task<IActionResult> Basket(int pId = 0)
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

        var discount = string.Empty;
        if (HttpContext.Session.GetString("discountCode") != null)
        {
            discount = HttpContext.Session.GetString("discountCode");
        }

        if (string.IsNullOrWhiteSpace(discount))
        {
            ViewBag.discount = new DiscountCode();
        }
        else
        {
            ViewBag.discount = await _work.GenericRepository<DiscountCode>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Code == discount);
        }

        ViewBag.Address = await _work.GenericRepository<UserAddress>().TableNoTracking
            .ToListAsync();
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

        ViewBag.Search = await _work.GenericRepository<SearchResult>().TableNoTracking.Take(6).ToListAsync();
        ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking.Take(4).ToListAsync();

        ViewBag.BasketProd = basketProducts;

        ViewBag.SelectPostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == pId) ?? new PostMethod();
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        ViewBag.NewProd = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory).ThenInclude(x => x.Category)
            .Include(x => x.Offer)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .OrderBy(x => x.InsertDate).Take(20).ToListAsync();
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
        var previousUrl = Request.Headers["Referer"];

        if (!string.IsNullOrEmpty(previousUrl))
        {
            return Redirect(previousUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> Login()
    {
        ViewBag.Seo = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync();
        return View();
    }

    public async Task<ActionResult> LoginPhone()
    {
        return View();
    }

    public async Task<ActionResult> ConfirmPhoneNumber(string phoneNumber, bool state = true)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(3);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(phoneNumber, user.ConfirmCode,
            "SignUpCode");
        @ViewBag.Phone = phoneNumber;
        ViewBag.Seo = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync();

        return View();
    }

    public async Task<ActionResult> Register()
    {
        return View();
    }

    public async Task<ActionResult> ConfirmCode(string phoneNumber)
    {
        ViewBag.Seo = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync();
        ViewBag.Phone = phoneNumber;
        return View();
    }

    public async Task<ActionResult> WelcomeParsme()
    {
        return View();
    }

    [HttpPost]
    public async Task<ApiAction> ValidateCode(string phoneNumber, string code)
    {
        // بررسی ورودی‌ها
        if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "Phone number and code are required."
            };
        }

        try
        {
            // ارسال درخواست برای تأیید کد
            var result = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
            // بررسی نتیجه تأیید کد
            if (result == null || !result.IsSuccess)
            {
                return result;
            }

            // اگر کد معتبر بود، اطلاعات کاربر را بازیابی کنید
            var user = await _userManager.FindByNameAsync(phoneNumber);

            if (user == null)
            {
                return result;
            }

            // انجام ورود کاربر
            var signInResult = await _signInManager.PasswordSignInAsync(user, user.Password, true, false);

            if (!signInResult.Succeeded)
            {
                return result;
            }

            // موفقیت آمیز بودن عملیات
            return result;
        }
        catch (Exception ex)
        {
            // مدیریت خطا
            return new ApiAction
            {
                IsSuccess = false,
                Message = "An error occurred while processing your request.",
            };
        }
    }

    [HttpPost]
    public async Task<ApiAction> ValidateCodeUser(string phoneNumber, string code)
    {
        // بررسی ورودی‌های الزامی
        if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
        {
            return new ApiAction
            {
                IsSuccess = false,
                Message = "شماره نامعتبر"
            };
        }

        try
        {
            // ارسال درخواست برای تایید کد
            var result = await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));

            var user = await _userManager.FindByNameAsync(phoneNumber);
            var signInResult = await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
            return result;
        }
        catch (Exception ex)
        {
            // مدیریت خطای داخلی
            return new ApiAction
            {
                IsSuccess = false,
                Message = "An error occurred while processing your request."
            };
        }
    }


    [HttpPost]
    public async Task<IActionResult> SendLoginCode(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        KavenegarApi webApi = new KavenegarApi(apikey: ApiKeys.ApiKey);
        if (user == null)
            return RedirectToAction("SignUp", "Home", new { phoneNumber });
        user.ConfirmCode = Helpers.GetConfirmCode();
        user.ConfirmCodeExpireTime = DateTime.Now.AddMinutes(3);
        await _userManager.UpdateAsync(user);
        var result = webApi.VerifyLookup(phoneNumber, user.ConfirmCode,
            "LoginCode");
        return RedirectToAction("ConfirmCode", new { user.PhoneNumber });
    }

    public async Task<ActionResult> ProfileDetail()
    {
        if (User.Identity.IsAuthenticated)
        {
            var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
                .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
                .ToListAsync();
            ViewBag.Categories = cats;
            ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                                 new FooterLink();

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
            ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                              new SeoPage();
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


    public async Task<IActionResult> WhyParsPage()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }


    public async Task<IActionResult> ParsAtGlance()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsGoals()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsBuyingGuide()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsOrganizationalPurchase()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsGuarantee()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsShippingMethods()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsConsultationBeforePurchase()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ProceduresForReturning()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> TrackingOrders()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }


    public async Task<IActionResult> ParsOnlineSupport()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ParsRulesAndRegulations()
    {
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();

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

        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        ViewBag.Pages = await _work.GenericRepository<FooterPage>().Table.FirstOrDefaultAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }

    public async Task<IActionResult> ShowFactor(int id)
    {
        var factor = await _work.GenericRepository<Factor>().TableNoTracking.Include(x => x.Products)
            .ThenInclude(x => x.FactorProductColor).ThenInclude(x => x.FactorProduct)
            .Include(x => x.User)
            .Include(x => x.UserAddress)
            .Include(x => x.PostMethod)
            .FirstOrDefaultAsync(x => x.Id == id);
        ViewBag.Factor = factor;
        return View();
    }
}