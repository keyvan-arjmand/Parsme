using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.Product;

namespace Domain.Entity.Factor.Product;

public class FactorProduct : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string PersianTitle { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ImageUri { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }
    public string UnicCode { get; set; } = string.Empty;
    public int? BrandId { get; set; } 
    [ForeignKey(nameof(BrandId))] public Brand? BrandEn { get; set; }
    public string Brand { get; set; } = string.Empty;
    public int Profit { get; set; }

    public int FactorId { get; set; }
    [ForeignKey(nameof(FactorId))] public Factor Factor { get; set; } = default!;
    public ICollection<FactorProductColor> FactorProductColor { get; set; } = default!;
}