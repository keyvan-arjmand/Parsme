using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Search;

namespace WebApp.Controllers;

public class SearchController : Controller
{
    private readonly IUnitOfWork _work;

    public SearchController(IUnitOfWork work)
    {
        _work = work;
    }

    [HttpGet]
    public async Task<List<SearchProd>> SearchProd(string search)
    {
        return await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.SubCategory)
            .Include(x => x.BrandTag)
            .Where(x => x.Title.Contains(search) || x.PersianTitle.Contains(search) || x.Brand.Title.Contains(search) ||
                        x.Detail.Contains(search) || x.MetaKeyword.Contains(search) || x.Code.Contains(search) ||
                        x.BrandTag.Title.Contains(search) || x.SeoTitle.Contains(search) ||
                        x.UnicCode.Contains(search) || x.SubCategory.Name.Contains(search))
            .Take(5).Select(x => new SearchProd
            {
                PersianTitle = x.PersianTitle,
                Id = x.Id,
                ImageUri = x.ImageUri
            }).ToListAsync();
    }
}