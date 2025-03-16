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

    private async void BackPage()
    {
        _steps = _steps switch
        {
            Steps.Step1 => Steps.Step1,
            Steps.Step2 => Steps.Step1,
            Steps.Step3 => Steps.Step2,
            Steps.Step4 => Steps.Step3,
            Steps.Step5 => Steps.Step4,
            Steps.Step6 => Steps.Step5,
            _ => Steps.Step1
        };
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