using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageColors
{
    [Inject] private IUnitOfWork _work { set; get; }
    private List<Color> _colors = [];

    protected override async Task OnInitializedAsync()
    {
        _colors = await _work.GenericRepository<Color>().TableNoTracking.ToListAsync();
    }
}