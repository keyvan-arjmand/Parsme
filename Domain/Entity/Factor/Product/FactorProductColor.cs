using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Factor.Product;

public class FactorProductColor:BaseEntity
{
    public double OfferAmount { get; set; }
    public int ColorId { get; set; } 
    public string ColorCode { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string Guarantee { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Count { get; set; }
    public int FactorProductId { get; set; }
    [ForeignKey(nameof(FactorProductId))] public FactorProduct FactorProduct { get; set; } = default!;
    
}