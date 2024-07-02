using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class SubCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category? Category { get; set; }
    public ICollection<CategoryDetail> CategoryDetails { get; private set; } = default!;
    public ICollection<Brand> Brands { get; private set; } = default!;
}