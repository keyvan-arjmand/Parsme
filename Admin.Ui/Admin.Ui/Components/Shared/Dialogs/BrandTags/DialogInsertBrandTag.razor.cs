using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace Admin.Ui.Components.Shared.Dialogs.BrandTags;

public partial class DialogInsertBrandTag
{
    [Inject] private IJSRuntime JSRuntime { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    private MudForm _form = new MudForm();
    private void Submit() => MudDialog.Close(DialogResult.Ok(true));
    private void Cancel() => MudDialog.Cancel();
    private void Close() => MudDialog.Close(DialogResult.Ok(false));
    private BrandTag _brandTag = new();
    private IBrowserFile _logo { set; get; }

    private async Task InsertVendorProduct()
    {
        await _form.Validate();
        if (!_form.IsValid)
        {
            await JSRuntime.InvokeVoidAsync("Swal.fire", new
            {
                icon = "error",
                title = "خطا",
                text = "لطفاً تمام فیلدهای الزامی را پر کنید."
            });
            return;
        }

        // var result = await _productVendorService.InsertAsync(new ProductVendorCreate
        // {
        //     PrsCount = Product.PrsCount,
        //     PrsDiscount = Product.PrsDiscount,
        //     PrsGuarantee = Product.PrsGuarantee,
        //     PrsPrice = Product.PrsPrice,
        //     PrsProductId = Id,
        //     PrsLimit = Product.PrsLimit,
        //     ColorId = _ColorId,
        // }, VendorId);


        if (true)
        {
            Submit();
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("Swal.fire", new
            {
                icon = "error",
                title = "خطا",
                text = "خطایی رخ داده است"
            });
        }
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        _logo = file;
    }
}