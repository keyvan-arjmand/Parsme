using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entity.Product;

public class CategoryDetail : BaseEntity
{
    public int FeatureId { get; set; }
    [ForeignKey("FeatureId")] public Feature? Feature { get; set; }
    public string Title { get; set; } = string.Empty;
    public DataType DataType { get; set; }
    public string Option { get; set; } = string.Empty;
    public bool ShowInSearch { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public ICollection<SubCategoryDetail> SubCategoryDetails { get; set; } = default!;
}