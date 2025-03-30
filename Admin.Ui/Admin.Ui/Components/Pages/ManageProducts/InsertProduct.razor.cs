using Admin.Ui.Models.Dtos;
using Application.Interfaces;
using Domain.Entity.Product;
using Domain.Migrations;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Admin.Ui.Components.Pages.ManageProducts;

public partial class InsertProduct
{
    [Inject] IUnitOfWork UnitOfWork { get; set; }
    private Product _product { set; get; } = new();
    private Steps _steps { set; get; } = Steps.Step1;
    private int _selectSubCategoryId { set; get; } = 0;
    private List<DetailProdAdmin> detailProdAdmins = [];
    private List<SubCategory> subCategories = [];
    private IEnumerable<int> selectedColors = [];
    private List<Color> colors = [];
    private List<Guarantee> guarantees = [];
    private List<GuaranteeIds> guaranteeIds = [];

    protected override async Task OnInitializedAsync()
    {
        guarantees = await UnitOfWork.GenericRepository<Guarantee>().TableNoTracking.ToListAsync();
        colors = await UnitOfWork.GenericRepository<Color>().TableNoTracking.ToListAsync();
        subCategories = await UnitOfWork.GenericRepository<SubCategory>().TableNoTracking.ToListAsync();
        StateHasChanged();
    }

    private async Task ChangeSubCategory(ChangeEventArgs e)
    {
        detailProdAdmins.Clear();
        _selectSubCategoryId = Convert.ToInt32(e.Value.ToString());
        var detail = await UnitOfWork.GenericRepository<CategoryDetail>()
            .TableNoTracking
            .Include(x => x.SubCategoryDetails).ThenInclude(x => x.SubCategory).ThenInclude(x => x.Category)
            .Include(x => x.Feature)
            .Where(x => x.SubCategoryDetails.Any(q => q.SubCategoryId == _selectSubCategoryId))
            .OrderByDescending(x => x.Feature.Priority).ToListAsync();
        foreach (var i in detail)
        {
            if (detailProdAdmins.Any(x => x.FeatureId == i.FeatureId))
            {
                var result = detailProdAdmins.FirstOrDefault(x => x.FeatureId == i.FeatureId);
                result.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option, DataType = (int)i.DataType, Priority = i.Priority, Title = i.Title,
                    FeatureId = i.FeatureId, ShowInSearch = i.ShowInSearch, SubCategoryId = _selectSubCategoryId,
                    Id = i.Id
                });
            }
            else
            {
                var li = new DetailProdAdmin
                {
                    Feature = i.Feature,
                    FeatureId = i.FeatureId
                };
                li.CategoryDetails.Add(new CategoryDetailDto
                {
                    Option = i.Option, DataType = (int)i.DataType, Priority = i.Priority, Title = i.Title,
                    FeatureId = i.FeatureId, ShowInSearch = i.ShowInSearch, SubCategoryId = _selectSubCategoryId,
                    Id = i.Id
                });
                detailProdAdmins.Add(li);
            }
        }

        Console.WriteLine(detailProdAdmins.Count);
        StateHasChanged();
    }

    private async void ChangeStep(Steps step)
    {
        _steps = step;
        StateHasChanged();
    }

    private async Task CreateProduct()
    {
        await UnitOfWork.GenericRepository<Product>().AddAsync(new Product(), CancellationToken.None);
    }

    private async void BackPage()
    {
        _steps = _steps switch
        {
            Steps.Step1 => Steps.Step1,
            Steps.Step2 => Steps.Step1,
            Steps.Step3 => Steps.Step2,
            Steps.Step4 => Steps.Step3,
            Steps.Step5 => Steps.Step4,
            Steps.Step6 => Steps.Step5,
            _ => Steps.Step1
        };
        StateHasChanged();
    }

    private enum Steps
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6
    }

    private async Task HandleContentChanged(string newContent)
    {
        _product.FullDesc = newContent;
    }

    private async void OnSelectedValuesChanged(IEnumerable<int>? values)
    {
        selectedColors = values.ToList();
        StateHasChanged();
        await Task.CompletedTask;
    }

    private async void OnGuValuesChanged(IEnumerable<string>? values)
    {
      
        foreach (var i in values)
        {
            var splited = i.Split(",");
            if (guaranteeIds.Any(x => x.Id == Convert.ToInt32(splited[1])))
            {
                guaranteeIds.FirstOrDefault(x => x.Id == Convert.ToInt32(splited[1])).Ids
                    .Add(Convert.ToInt32(splited[0]));
            }
            else
            {
                guaranteeIds.Add(new GuaranteeIds
                {
                    Id = Convert.ToInt32(splited[1]),
                    Ids = [Convert.ToInt32(splited[0])]
                });
            }
        }

        Console.WriteLine(JsonConvert.SerializeObject(guaranteeIds));
        StateHasChanged();
        await Task.CompletedTask;
    }

    private class GuaranteeIds
    {
        public int Id { get; set; }
        public List<int> Ids { get; set; }
    }
}