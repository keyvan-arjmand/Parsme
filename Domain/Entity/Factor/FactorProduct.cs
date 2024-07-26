using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Factor;

public class FactorProduct:BaseEntity
{
    public int? FactorId { get; set; }
    [ForeignKey("FactorId")] public Factor? Factor { get; set; }
    public int? ProductId { get; set; }
    [ForeignKey("ProductId")] public Product.Product? Product { get; set; }
}