using Admin.Ui.Client.Models.Dtos;
using Admin.Ui.Client.Shared.Dialogs.BrandTags;
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
    [Inject] private IJSRuntime JSRuntime{get;set;}

    [Inject] private IDialogService DialogService { set; get; }
    [Inject] private IUnitOfWork _work { set; get; }
    [Inject] IWebHostEnvironment _env { set; get; }
    
    private List<BrandTag> _brands = [];
    private BrandTagDto _brand = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        _brands = await _work.GenericRepository<BrandTag>().TableNoTracking.ToListAsync();
    }
    protected override void OnInitialized()
    {
    }

    private async void OpenInsertDialog()
    {
        var dialogRef = await DialogService.ShowAsync<DialogInsertBrandTag>("");
        var result = await dialogRef.Result;

        if (!result.Canceled)
        {
            if ((bool)result.Data)
            {
                Snackbar.Add("عملیات با موفقیت انجام شد", Severity.Success);
            }
        }
    }

    private MudMessageBox _mudMessageBox;

    private async Task InsertBrand()
    {
        Uploader up = new Uploader(_env);
        await _work.GenericRepository<BrandTag>().AddAsync(new BrandTag
        {
            IsClick = _brand.IsClick,
            Title = _brand.Title,
            LogoUri =await up.UploadBrowserFile(_brand.LogoUriFile,"brand"),
        }, CancellationToken.None);
        await JSRuntime.InvokeVoidAsync("closeModal", "modal-add-brand");
        await JSRuntime.InvokeVoidAsync("Swal.fire", new
        {
            icon = "success",
            title = "موفق",
            text = "برند با موفقیت افزوده شد"
        });
        await LoadData();
    }

    private void UploadFiles(IBrowserFile file)
    {
        _brand.LogoUriFile = file;
    }
}