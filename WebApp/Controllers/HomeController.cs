using System.Diagnostics;
using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _work;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork work)
    {
        _logger = logger;
        _work = work;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.ProductColors)
            .ToListAsync();
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        return View();
    }

    public async Task<IActionResult> ProductPage(int id)
    {
        ViewBag.Categories = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .ThenInclude(x => x.Brands)
            .ToListAsync();
        var product = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.Offer)
            .Include(x => x.ProductDetails)
            .Include(x => x.ProductImages)
            .Include(x => x.SubCategory).ThenInclude(x => x.Category)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefaultAsync(x => x.Id == id);
        foreach (var i in product.ProductDetails)
        {
            i.CategoryDetail = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Id == i.CategoryDetailId);
        }

        ViewBag.Product = product;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}