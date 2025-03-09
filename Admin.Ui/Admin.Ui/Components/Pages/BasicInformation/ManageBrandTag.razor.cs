using Admin.Ui.Client.Shared.Dialogs.BrandTags;
using Admin.Ui.Models.Dtos;
using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageBrandTag
{
    [Inject] private ISnackbar Snackbar { set; get; }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    [Inject] private IDialogService DialogService { set; get; }
    [Inject] private IUnitOfWork _work { set; get; }
    [Inject] IWebHostEnvironment _env { set; get; }

    private List<BrandTag> _brands = [];
    private BrandTagDto _brand = new();
    private BrandTagDto _brandEdit = new();
    private PageableParams _filter = new();
    private bool _isLoading = true;
    private string _logoPreview;
    private MudMessageBox _mudMessageBox;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async void EditBrand(BrandTag brandEdit)
    {
        _logoPreview = "https://newdev.parsme.com/Images/Brand/" + brandEdit.LogoUri;
        _brandEdit = new BrandTagDto
        {
            IsClick = brandEdit.IsClick,
            Title = brandEdit.Title,
            LogoUri = brandEdit.LogoUri,
            Id = brandEdit.Id,
        };
        StateHasChanged();
    }

    private async Task LoadData()
    {
        _isLoading = true;
        StateHasChanged();

        var query = _work.GenericRepository<BrandTag>().TableNoTracking.AsNoTracking();

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
            .AsNoTracking()
            .ToListAsync();

        _isLoading = false;
        StateHasChanged();
    }


    private async void OpenInsertDialog()
    {
        _logoPreview = string.Empty;
        StateHasChanged();
    }

    private async Task InsertBrand()
    {
        Uploader up = new Uploader(_env);
        await _work.GenericRepository<BrandTag>().AddAsync(new BrandTag
        {
            IsClick = _brand.IsClick,
            Title = _brand.Title,
            LogoUri = await up.UploadBrowserFile(_brand.LogoUriFile, "brand"),
        }, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalCenter");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadData();
    }

    private async Task UpdateBrand()
    {
        Uploader up = new Uploader(_env);
        var image = await up.UploadBrowserFile(_brandEdit.LogoUriFile, "brand");
        var entity = await _work.GenericRepository<BrandTag>().Table.FirstOrDefaultAsync(x => x.Id == _brandEdit.Id);
        entity.Title = _brandEdit.Title;
        entity.LogoUri = string.IsNullOrWhiteSpace(image) ? entity.LogoUri : image;
        entity.IsClick = _brandEdit.IsClick;
        await _work.GenericRepository<BrandTag>().UpdateAsync(entity, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modalCenterEdit");
        await JSRuntime.InvokeVoidAsync("showToast");
        await LoadData();
    }

    private async void UploadFiles(IBrowserFile file)
    {
        _brand.LogoUriFile = file;
        if (file != null)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            _logoPreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        StateHasChanged();
    }

    private async void UploadFilesEdite(IBrowserFile file)
    {
        _brandEdit.LogoUriFile = file;
        if (file != null)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            _brandEdit.LogoUri = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
            _logoPreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        StateHasChanged();
    }

    private async Task DeleteBrand(int id)
    {
        var brand = await _work.GenericRepository<BrandTag>().Table.FirstOrDefaultAsync(x => x.Id == id);
        brand.IsDelete = true;
        await _work.GenericRepository<BrandTag>().UpdateAsync(brand, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("showToastDelete");
        await LoadData();
    }

    private async Task OnPageChanged(int newPage)
    {
        _filter.Page = newPage;
        await LoadData();
    }

    private async Task OnSearchChanged(string text)
    {
        _filter.Search = text;
        await LoadData();
    }
}