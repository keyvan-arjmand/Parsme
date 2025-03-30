using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductDetail : BaseEntity
{ 
    public int CategoryDetailId { get; set; }
    [ForeignKey(nameof(CategoryDetailId))] public CategoryDetail? CategoryDetail { get; set; }
    public int ProductId { get; set; }
 
    public string? Value { get; set; }
}