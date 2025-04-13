using Admin.Ui.Models.Dtos;
using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageCategory
{
    [Inject] private IUnitOfWork _work { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private MainCategory _mainCat { get; set; } = new();
    private Category _cat { get; set; } = new();
    private SubCategory _subCat { get; set; } = new();

    private List<MainCategory> _mainForInsert = new();
    private List<Category> _catForInsert = new();

    private List<MainCategory> _mainCategory = new();
    private List<Category> _category = new();
    private List<SubCategory> _subCategory = new();

    private PageableParams _mainCatFilter = new();
    private PageableParams _catFilter = new();
    private PageableParams _subCatFilter = new();

    private bool _MainCatLoading = false;
    private bool _CatLoading = false;
    private bool _SubCatLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _mainForInsert = await _work.GenericRepository<MainCategory>().TableNoTracking.ToListAsync();
        _catForInsert = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        await LoadMainCatData();
        await LoadCatData();
        await LoadSubCatData();
    }

    private async Task LoadMainCatData()
    {
        _MainCatLoading = true;
        StateHasChanged();

        var query = _work.GenericRepository<MainCategory>().TableNoTracking.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(_mainCatFilter.Search))
        {
            query = query.Where(x => x.Name.Contains(_mainCatFilter.Search));
        }

        var totalRecords = await query.CountAsync();
        _mainCatFilter.TotalPage = (int)Math.Ceiling((double)totalRecords / _mainCatFilter.PageSize);

        _mainCategory = await query
            .OrderByDescending(x => x.Id)
            .Skip((_mainCatFilter.Page - 1) * _mainCatFilter.PageSize)
            .Take(_mainCatFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        _MainCatLoading = false;
        StateHasChanged();
    }

    private async Task LoadCatData()
    {
        _CatLoading = true;
        StateHasChanged();

        var query = _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.SubCategories)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(_catFilter.Search))
        {
            query = query.Where(x => x.Name.Contains(_catFilter.Search));
        }

        var totalRecords = await query.CountAsync();
        _catFilter.TotalPage = (int)Math.Ceiling((double)totalRecords / _catFilter.PageSize);

        _category = await query
            .OrderByDescending(x => x.Id)
            .Skip((_catFilter.Page - 1) * _catFilter.PageSize)
            .Take(_catFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        _CatLoading = false;
        StateHasChanged();
    }

    private async Task LoadSubCatData()
    {
        _SubCatLoading = true;
        StateHasChanged();

        var query = _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(_subCatFilter.Search))
        {
            query = query.Where(x => x.Name.Contains(_subCatFilter.Search));
        }

        var totalRecords = await query.CountAsync();
        _subCatFilter.TotalPage = (int)Math.Ceiling((double)totalRecords / _subCatFilter.PageSize);

        _subCategory = await query
            .OrderByDescending(x => x.Id)
            .Skip((_subCatFilter.Page - 1) * _subCatFilter.PageSize)
            .Take(_subCatFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        _SubCatLoading = false;
        StateHasChanged();
    }

    private async Task OnPageMainChanged(int newPage)
    {
        _mainCatFilter.Page = newPage;
        await LoadMainCatData();
    }

    private async Task OnPageCatChanged(int newPage)
    {
        _catFilter.Page = newPage;
        await LoadCatData();
    }

    private async Task OnPageSubChanged(int newPage)
    {
        _subCatFilter.Page = newPage;
        await LoadSubCatData();
    }

    private async Task InsertMainCategory()
    {
        await _work.GenericRepository<MainCategory>().AddAsync(_mainCat, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalMain");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }

    private async Task InsertCategory()
    {
        await _work.GenericRepository<Category>().AddAsync(_cat, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalCat");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }

    private async Task InsertSubCategory()
    {
        await _work.GenericRepository<SubCategory>().AddAsync(_subCat, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalSubCat");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }

    private async Task DeleteMainCategory(int id)
    {
        var result = await _work.GenericRepository<MainCategory>().TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
        result.IsDelete = true;
        await _work.GenericRepository<MainCategory>().UpdateAsync(result, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalMain");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }

    private async Task DeleteCategory(int id)
    {
        var result = await _work.GenericRepository<Category>().TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
        result.IsDelete = true;
        await _work.GenericRepository<Category>().UpdateAsync(result, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalCat");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }

    private async Task DeleteSubCategory(int id)
    {
        var result = await _work.GenericRepository<SubCategory>().TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
        result.IsDelete = true;
        await _work.GenericRepository<SubCategory>().UpdateAsync(result, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalSubCat");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadMainCatData();
    }
}