using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entity.Product;

public class CategoryDetail : BaseEntity
{
    public int SubCategoryId { get; set; }
    [ForeignKey("SubCategoryId")] public SubCategory? SubCategory { get; set; }
    public int FeatureId { get; set; }
    [ForeignKey("FeatureId")] public Feature? Feature { get; set; }
    public string Title { get; set; } = string.Empty;
    public DataType DataType { get; set; }
    public string Option { get; set; } = string.Empty;
    public bool ShowInSearch { get; set; }
    public int Priority { get; set; }
}