using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageGuarantee
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<Guarantee> _guarantees = new();

    protected override async Task OnInitializedAsync()
    {
        _guarantees = await _work.GenericRepository<Guarantee>().TableNoTracking.ToListAsync();
    }
}