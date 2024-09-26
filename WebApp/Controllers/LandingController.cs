using Application.Interfaces;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp.Controllers;

public class LandingController : Controller
{
    private readonly IUnitOfWork _work;

    public LandingController(IUnitOfWork work)
    {
        _work = work;
    }

    // GET
    public async Task<IActionResult> Index(int id)
    {
        if (!await _work.GenericRepository<BrandTag>().TableNoTracking.AnyAsync(x => x.Id == id)) throw new Exception();
        ViewBag.Id = id;
        var cats = await _work.GenericRepository<MainCategory>().TableNoTracking
            .Include(x => x.Categories).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Brands)
            .ToListAsync();
        ViewBag.Categories = cats;
        ViewBag.FooterLink = await _work.GenericRepository<FooterLink>().TableNoTracking.FirstOrDefaultAsync() ??
                             new FooterLink();
        ViewBag.Products = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.Offer)
            .Where(x => x.BrandTagId == id).ToListAsync();
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

        ViewBag.SubCats = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Brands)
            .ToListAsync();
        ViewBag.Landing = await _work.GenericRepository<BrandLanding>().TableNoTracking
            .FirstOrDefaultAsync(x => x.BrandTagId == id) ?? new();
        ViewBag.BasketProd = basketProducts;
        ViewBag.SeoPage = await _work.GenericRepository<SeoPage>().TableNoTracking.FirstOrDefaultAsync() ??
                          new SeoPage();
        return View();
    }
}