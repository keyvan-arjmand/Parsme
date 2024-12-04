using Application.Interfaces;
using Domain.DataBase;
using Domain.Entity.Product;
using Domain.Entity.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Search;

namespace WebApp.Controllers;

public class SearchController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly AppDbContext _context;

    public SearchController(IUnitOfWork work, AppDbContext context)
    {
        _work = work;
        _context = context;
    }

    [HttpGet]
    public async Task<List<SearchProd>> SearchProd(string search)
    {
        var searchKeywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var query = _work.GenericRepository<Product>().TableNoTracking
            .Include(p => p.ProductColors) // Include ProductColors
            .ThenInclude(pc => pc.Color) // ThenInclude for Color inside ProductColors
            .Include(p => p.SubCategory) // Include SubCategory
            .ThenInclude(sc => sc.Category) // ThenInclude for Category inside SubCategory
            .Include(p => p.Offer).AsQueryable();
        ;
        if (searchKeywords.Any())
        {
            foreach (var keyword in searchKeywords)
            {
                query = query.Where(product =>
                    product.Title.Contains(keyword) ||
                    product.PersianTitle.Contains(keyword) ||
                    product.Brand.Title.Contains(keyword) ||
                    product.Detail.Contains(keyword) ||
                    product.MetaKeyword.Contains(keyword) ||
                    product.BrandTag.Title.Contains(keyword) ||
                    product.SeoTitle.Contains(keyword) ||
                    product.UnicCode.Contains(keyword) ||
                    product.SubCategory.Name.Contains(keyword));
            }
        }

        return await query
            .Take(5)
            .Select(x => new SearchProd
            {
                PersianTitle = x.PersianTitle,
                ImageUri = x.ImageUri,
                Id = x.Id
            })
            .ToListAsync();
    }

    [HttpGet]
    public async Task<IActionResult> GetCities(int stateId)
    {
        var cities = await _work.GenericRepository<City>().TableNoTracking.Where(x => x.StateId == stateId)
            .ToListAsync();
        return Ok(cities);
    }
}