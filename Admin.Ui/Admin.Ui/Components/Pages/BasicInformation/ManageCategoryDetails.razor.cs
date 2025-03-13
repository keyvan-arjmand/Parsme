using Application.Interfaces;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Admin.Ui.Components.Pages.BasicInformation;

public partial class ManageCategoryDetails
{
    [Inject] private IUnitOfWork _work { set; get; }
         private List<CategoryDetail> _categoryDetails = [];
         private List<Feature> _features = [];
     
         protected override async Task OnInitializedAsync()
         {
             _categoryDetails = await _work.GenericRepository<CategoryDetail>().TableNoTracking
                 .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
                 .Include(x => x.Feature)
                 .OrderByDescending(x => x.Id).ToListAsync();
             _features = await _work.GenericRepository<Feature>().TableNoTracking
                 .OrderByDescending(x => x.Id)
                 .ToListAsync();
         }
}