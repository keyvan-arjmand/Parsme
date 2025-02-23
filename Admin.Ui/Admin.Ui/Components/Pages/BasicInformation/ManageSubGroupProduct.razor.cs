using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageSubGroupProduct
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<Brand> _brands = new();

    protected override async Task OnInitializedAsync()
    {
        _brands = await _work.GenericRepository<Brand>().TableNoTracking
            .Include(x => x.SubCategory)
            .Include(x => x.Products)
            .AsSplitQuery()
            .ToListAsync();
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
    }
}