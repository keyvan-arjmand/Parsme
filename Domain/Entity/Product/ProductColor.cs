using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductColor : BaseEntity
{
    public int Priority { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product? Product { get; set; }
    public int ColorId { get; set; }
    [ForeignKey(nameof(ColorId))] public Color? Color { get; set; }

    public ICollection<ColorGuarantee> ColorGuarantees { get; set; } = null!;
}