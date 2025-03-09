using Domain.Entity.Product;
using Domain.Migrations;

namespace Admin.Ui.Components.Pages.ManageProducts;

public partial class InsertProduct
{
    private Product _product { set; get; } = new();
    private Steps _steps { set; get; } = Steps.Step1;

    private async void ChangeStep(Steps step)
    {
        _steps = step;
        StateHasChanged();
    }

    private enum Steps
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6
    }
}