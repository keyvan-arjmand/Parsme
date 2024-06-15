using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductColor : BaseEntity
{
    public double Price { get; set; }//RIT
    public int Priority { get; set; }
    public string ImageUri { get; set; } = string.Empty;
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; } = new();
    public int ColorId { get; set; }
    [ForeignKey("ColorId")] public Color Color { get; set; } = new();
    public int GuaranteeId { get; set; }
    [ForeignKey("GuaranteeId")] public Guarantee Guarantee { get; set; } = new();

}