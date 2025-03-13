using Admin.Ui.Models.Dtos;
using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageSubGroupProduct
{
    [Inject] private IUnitOfWork _work { set; get; }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private List<Brand> _brands = new();
    private PageableParams _filter = new();
    private bool _isLoading = false;
    private Brand? _selectedBrand = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        _isLoading = true;
        StateHasChanged(); 

        var query = _work.GenericRepository<Brand>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.Products)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(_filter.Search))
        {
            query = query.Where(x => x.Title.Contains(_filter.Search));
        }

        var totalRecords = await query.CountAsync();
        _filter.TotalPage = (int)Math.Ceiling((double)totalRecords / _filter.PageSize);

        _brands = await query
            .OrderByDescending(x => x.Id)
            .Skip((_filter.Page - 1) * _filter.PageSize)
            .Take(_filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        _isLoading = false;
        StateHasChanged(); 
    }

    private async Task OnSearchChanged(string text)
    {
        _isLoading = true;
        StateHasChanged();
        _filter.Search = text;
        _filter.Page = 1;
        await LoadData();
    }

    private async Task OnPageChanged(int newPage)
    {
        if (_filter.Page == newPage) return;
        _isLoading = true;
        StateHasChanged();
        _filter.Page = newPage;
        await LoadData();
    }

    private async Task InsertBrand()
    {
        await _work.GenericRepository<Brand>().AddAsync(new Brand
        {
            Title = _selectedBrand.Title,
            SubCategoryId = _selectedBrand.SubCategoryId,
            LogoUri = _selectedBrand.LogoUri,
            IsActive = _selectedBrand.IsActive,
            SeoCanonical = _selectedBrand.SeoCanonical,
            SeoTitle = _selectedBrand.SeoTitle,
            SeoDesc = _selectedBrand.SeoDesc,
            OnClick = 0,
        }, CancellationToken.None);

        await JSRuntime.InvokeVoidAsync("closeModal", "modalCenter");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadData();
    }

    private async void DeleteBrand(int id)
    {
        var brand = await _work.GenericRepository<Brand>().Table.FirstOrDefaultAsync(x => x.Id == id);
        brand.IsDelete = true;
        await _work.GenericRepository<Brand>().UpdateAsync(brand, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("showToastDelete");
        await LoadData();
    }

    private async void EditBrand(Brand brandEdit)
    {
        _selectedBrand = new Brand
        {
            Title = brandEdit.Title,
            LogoUri = brandEdit.LogoUri,
            Id = brandEdit.Id,
            SubCategoryId = brandEdit.SubCategoryId,
            IsActive = brandEdit.IsActive,
            SeoCanonical = brandEdit.SeoCanonical,
            SeoTitle = brandEdit.SeoTitle,
            SeoDesc = brandEdit.SeoDesc,
            OnClick = brandEdit.OnClick,
        };
        StateHasChanged();
    }
}