using System.Diagnostics;
using Application.Interfaces;
using Domain.Entity.Product;
using Domain.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _work;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public HomeController(ILogger<HomeController> logger, IUnitOfWork work, SignInManager<User> signInManager, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _logger = logger;
        _work = work;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            foreach (var id in basketList)
            {
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x=>x.Color)
                    .FirstOrDefaultAsync(x => x.Id == id);
                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .FirstOrDefaultAsync(x => x.Id==prodColor.ProductId);
                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }

        ViewBag.BasketProd = basketProducts;

        ViewBag.ProdCatalog = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory).Where(x => x.IsShowIndex)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Take(20).ToListAsync();

        ViewBag.NewProd = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .Include(x => x.SubCategory)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .OrderBy(x => x.InsertDate).Take(20).ToListAsync();
        ViewBag.OfferMoments =
            await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Where(x => x.MomentaryOffer).Take(7).ToListAsync();

        ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .ToListAsync();

        var offer = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.ProductColors)
            .Include(x => x.Offer)
            .Include(x => x.ProductDetails).ThenInclude(x => x.CategoryDetail).Where(x => x.IsOffer).Take(7)
            .ToListAsync();

        // foreach (var i in offer.ProductDetails)
        // {
        //     i.CategoryDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
        //         .FirstOrDefaultAsync(x => x.Id == i.CategoryDetailId);
        // }

        ViewBag.Offer = offer;
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        return View();
    }
    public async Task<IActionResult > UserLogin(string number, string password)
    {
        var user = await _userManager.FindByNameAsync(number);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
            return  RedirectToAction("Profile","Home");
        }
        else
        {
            return  RedirectToAction("Index","Home");
        }
    }
    public async Task<IActionResult > Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            
            return View();
        }
        else
        {
            return  RedirectToAction("Index","Home");
        }
    }
    public async Task<IActionResult> ProductPage(int id)
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
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
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x=>x.Color)
                    .FirstOrDefaultAsync(x => x.Id == i);
                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .FirstOrDefaultAsync(x => x.Id==prodColor.ProductId);
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
        }

        basketlist.Add(id);
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> Basket()
    {
        var basketProducts = new List<Product>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("basket")).ToList();
            foreach (var i in basketList)
            {
                var prodColor = await _work.GenericRepository<ProductColor>().TableNoTracking.Include(x=>x.Color)
                    .Include(x=>x.Guarantee)
                    .FirstOrDefaultAsync(x => x.Id == i);
                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                    .Include(x=>x.ProductColors).ThenInclude(x=>x.Guarantee)
                    .FirstOrDefaultAsync(x => x.Id==prodColor.ProductId);
                prod.ProductColors = new List<ProductColor>() { prodColor };
                basketProducts.Add(prod!);
            }
        }
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.BasketProd = basketProducts;
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
        return  RedirectToAction("Index","Home");
    }
    public async Task<ActionResult> Login()
    {
       
        return View();
    }
    public async Task<ActionResult> Register()
    {
        return View();
    }
    public async Task<IActionResult > UserRegister(string number, string password)
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
            return  RedirectToAction("Login");
        }
        else
        {
            return  RedirectToAction("Login");
        }
       
    }
}