using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class Offer : BaseEntity
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public DateTime StartDate { get; set; }
    public int ColorId { get; set; }
    [ForeignKey(nameof(ColorId))] public Color? Color { get; set; }
    public int? ProductId { get; set; }
    public double OfferAmount { get; set; }
}