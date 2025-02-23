using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageBrandTag
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<BrandTag> _brands = new();

    protected override async Task OnInitializedAsync()
    {
        _brands = await _work.GenericRepository<BrandTag>().TableNoTracking.ToListAsync();
    }
}