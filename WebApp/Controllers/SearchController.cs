using Application.Interfaces;
using Domain.DataBase;
using Domain.Entity.Product;
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
        List<SearchProd> searchProds = new List<SearchProd>();
        foreach (var i in searchKeywords)
        {
            searchProds.AddRange(await _work.GenericRepository<Product>().TableNoTracking
                .Include(x => x.Brand)
                .Include(x => x.SubCategory)
                .Include(x => x.BrandTag)
                .Where(x => x.Title.Contains(i) || x.PersianTitle.Contains(i) ||
                            x.Brand.Title.Contains(i) ||
                            x.Detail.Contains(i) || x.MetaKeyword.Contains(i) || 
                            x.BrandTag.Title.Contains(i) || x.SeoTitle.Contains(i) ||
                            x.UnicCode.Contains(i) || x.SubCategory.Name.Contains(i))
                .Take(5).Select(x => new SearchProd
                {
                    PersianTitle = x.PersianTitle,
                    Id = x.Id,
                    ImageUri = x.ImageUri
                }).ToListAsync());
        }

        return searchProds.DistinctBy(x => x.Id).ToList();
    }
}