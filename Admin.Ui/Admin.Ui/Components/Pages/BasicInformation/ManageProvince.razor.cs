using Application.Interfaces;
using Domain.Entity.User;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageProvince
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<State> _states = [];
    private List<City> _cities = [];

    protected override async Task OnInitializedAsync()
    {
        _states = await _work.GenericRepository<State>().TableNoTracking
            .Include(x => x.Cities)
            .OrderByDescending(x => x.Id).ToListAsync();
        _cities = await _work.GenericRepository<City>().TableNoTracking
            .Include(x => x.State)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }
}