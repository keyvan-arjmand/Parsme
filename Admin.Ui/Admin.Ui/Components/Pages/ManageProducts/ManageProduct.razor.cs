using Admin.Ui.Models.Dtos;
using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.ManageProducts;

public partial class ManageProduct
{
    [Inject] private IUnitOfWork _work { get; set; }
    private List<Product> _products = [];
    private PageableParams _filter = new PageableParams
    {
        PageSize = 11,
        Page = 1,
        Search = string.Empty,
        TotalPage = 0
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        _products = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.BrandTag)
            .Include(x => x.UserUpdate)
            .Include(x => x.Creator)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.ProductImages)
            .AsSplitQuery()
            .AsQueryable()
            .OrderByDescending(x => x.Id)
            .Skip((_filter.Page - 1) * _filter.PageSize)
            .Take(_filter.PageSize)
            .ToListAsync();
    }
}