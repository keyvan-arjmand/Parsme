using Domain.Entity.Product;

namespace Panel.Models;

public class DetailProdAdmin
{
    public int FeatureId { get; set; } 
    public Feature? Feature { get; set; }
    public List<CategoryDetailDto> CategoryDetails { get; set; } = new();
}

public class CategoryDetailDto
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public int FeatureId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DataType { get; set; }
    public string Option { get; set; } = string.Empty;
    public bool ShowInSearch { get; set; }
    public int Priority { get; set; }
}
