using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class SubCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? CategoryId { get; set; }
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
    [ForeignKey("CategoryId")] public Category? Category { get; set; }
    public ICollection<CategoryDetail> CategoryDetails { get; private set; } = default!;
    public ICollection<Brand> Brands { get; private set; } = default!;
}