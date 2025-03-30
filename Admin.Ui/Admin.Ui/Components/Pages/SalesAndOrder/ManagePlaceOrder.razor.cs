using Application.Interfaces;
using Domain.Entity.Factor;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.SalesAndOrder;

public partial class ManagePlaceOrder
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<Factor> _factors = [];

    protected override async Task OnInitializedAsync()
    {
        _factors = await _work.GenericRepository<Factor>().TableNoTracking.Include(x => x.User)
            .Include(x => x.PostMethod)
            .Include(x => x.UserAddress)
            .Include(x => x.Products)
            .ThenInclude(x => x.FactorProductColor)
            .OrderByDescending(x => x.InsertDate)
            .ToListAsync();
    }
}