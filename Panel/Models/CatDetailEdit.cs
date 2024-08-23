using Domain.Entity.Product;

namespace Panel.Models;

public class CatDetailEdit
{
    public int FeatureId { get; set; } 
    public Feature? Feature { get; set; }
    public List<CategoryDetailDtoEdit> CategoryDetails { get; set; } = new();
}

public class CategoryDetailDtoEdit
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public int FeatureId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DataType { get; set; }
    public string Option { get; set; } = string.Empty;
    public bool ShowInSearch { get; set; }
    public int Priority { get; set; }
    public string Value { get; set; }

}
