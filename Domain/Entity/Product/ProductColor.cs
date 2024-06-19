using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductColor : BaseEntity
{
    public double Price { get; set; }//RIT
    public int Priority { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; } = default!;
    public int ColorId { get; set; }
    [ForeignKey("ColorId")] public Color Color { get; set; } = default!;
    public int GuaranteeId { get; set; }
    [ForeignKey("GuaranteeId")] public Guarantee Guarantee { get; set; } = default!;
    public int Inventory { get; set; }
    //offer Product
    public bool IsOffer { get; set; }
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public double OfferAmount { get; set; }

}