using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageCategory
{
    [Inject] private IUnitOfWork _work { get; set; }
    private List<MainCategory> _mainCategory = new();
    private List<Category> _category = new();
    private List<SubCategory> _subCategory = new();

    protected override async Task OnInitializedAsync()
    {
        _mainCategory = await _work.GenericRepository<MainCategory>().TableNoTracking.ToListAsync();

        _category = await _work.GenericRepository<Category>().TableNoTracking
            .Include(x => x.MainCategory)
            .ToListAsync();
        _subCategory = await _work.GenericRepository<SubCategory>().TableNoTracking
            .Include(x => x.Category)
            .ToListAsync();
    }
}