using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ColorGuarantee:BaseEntity
{
    public int ProductColorId { get; set; }
    [ForeignKey(nameof(ProductColorId))] public ProductColor? ProductColor { get; set; }
    public int GuaranteeId { get; set; }
    [ForeignKey(nameof(GuaranteeId))] public Guarantee? Guarantee { get; set; }
    public double Price { get; set; }
    public int Inventory { get; set; }
}