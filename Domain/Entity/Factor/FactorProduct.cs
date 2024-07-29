using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.Product;

namespace Domain.Entity.Factor;

public class FactorProduct : BaseEntity
{
    public int? FactorId { get; set; }
    [ForeignKey("FactorId")] public Factor? Factor { get; set; }
    public int? ProductColorId { get; set; }
    [ForeignKey("ProductColorId")] public ProductColor? ProductColor { get; set; }
    public int Count { get; set; }
}