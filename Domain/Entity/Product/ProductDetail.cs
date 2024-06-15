using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductDetail : BaseEntity
{
    public int CategoryDetailId { get; set; }
    [ForeignKey("CategoryDetailId")] public CategoryDetail CategoryDetail { get; set; } = new();
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; } = new ();
    public string Value { get; set; } = string.Empty;
}