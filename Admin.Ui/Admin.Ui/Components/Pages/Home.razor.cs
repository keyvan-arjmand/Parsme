using Admin.Ui.Models.Reports;
using Application.Interfaces;
using Domain.Entity.Factor;
using Domain.Entity.User;
using Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages;

public partial class Home
{
    [Inject] private IUnitOfWork _work { set; get; }
    [Inject] private UserManager<User> _userManager { set; get; }
    private ReportDashboard _reportDashboard = new();

    protected override async Task OnInitializedAsync()
    {
        _reportDashboard = new ReportDashboard
        {
            AllUserCount = await _userManager.Users.CountAsync(),
            AllIncome = await _work.GenericRepository<Factor>().TableNoTracking.SumAsync(x => x.Amount),
            NewOrderCount = await _work.GenericRepository<Factor>().TableNoTracking
                .CountAsync(x => x.Status == Status.Pending && x.InsertDate.Date == DateTime.Now.Date),
            TodayOrderCount = await _work.GenericRepository<Factor>().TableNoTracking
                .CountAsync(x => x.InsertDate.Date == DateTime.Now.Date)
        };
    }
}