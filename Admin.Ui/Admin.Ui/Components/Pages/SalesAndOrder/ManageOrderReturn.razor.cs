using Application.Interfaces;
using Domain.Entity.Factor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.SalesAndOrder;

public partial class ManageOrderReturn
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<ReturnedFactor> _factors = [];

    protected override async Task OnInitializedAsync()
    {
        _factors = await _work.GenericRepository<ReturnedFactor>().TableNoTracking
            .Include(x => x.Factor).ThenInclude(x => x.User)
            .Include(x => x.Factor).ThenInclude(x => x.PostMethod)
            .Include(x => x.Factor).ThenInclude(x => x.UserAddress).ThenInclude(q => q.City)
            .Include(x => x.Factor).ThenInclude(x => x.Products)
            .ThenInclude(x => x.FactorProductColor)
            .Include(x => x.Factor).ThenInclude(x => x.Products)
            .ThenInclude(x => x.FactorProductColor)
            .AsSplitQuery()
            .OrderByDescending(x => x.InsertDate)
            .ToListAsync();
    }
}