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
    private bool _isLoading = false;
   

    private PageableParams _filter = new PageableParams
    {
        PageSize = 10,
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
        _isLoading = true;
        StateHasChanged();
        var query = _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Brand)
            .Include(x => x.BrandTag)
            .Include(x => x.UserUpdate)
            .Include(x => x.Creator)
            .Include(x => x.ProductColors).ThenInclude(x => x.Color)
            .Include(x => x.ProductImages)
            .AsSplitQuery()
            .AsQueryable();
        if (!string.IsNullOrEmpty(_filter.Search))
        {
            query = query
                .Where(x => x.SubCategory.Name.Contains(_filter.Search) ||
                            x.Brand.Title.Contains(_filter.Search) ||
                            x.Detail.Contains(_filter.Search) ||
                            x.Strengths.Contains(_filter.Search) ||
                            x.FullDesc.Contains(_filter.Search) ||
                            x.MetaKeyword.Contains(_filter.Search) ||
                            x.FullDesc.Contains(_filter.Search) ||
                            x.PersianTitle.Contains(_filter.Search) ||
                            x.WeakPoints.Contains(_filter.Search) ||
                            x.ProductGift.Contains(_filter.Search));
            // x.ProductColors.Any(t => t.Color.ColorCode.Contains(_filter.Search) ||
            //                          t.Color.Title.Contains(_filter.Search)) ||
            //     x.ProductDetails.Any(q => q.Value.Contains(_filter.Search))
        }

        _products = await query.OrderByDescending(x => x.Id)
            .Skip((_filter.Page - 1) * _filter.PageSize)
            .Take(_filter.PageSize)
            .ToListAsync();
        var totalRecords = await query.CountAsync();
        _filter.TotalPage = (int)Math.Ceiling((double)totalRecords / _filter.PageSize);
        _isLoading = false;
        StateHasChanged();
    }

    private async Task OnPageChanged(int newPage)
    {
        _filter.Page = newPage;
        Console.WriteLine(_filter.Page);
        await LoadData();
    }
}