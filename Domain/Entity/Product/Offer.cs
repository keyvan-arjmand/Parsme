using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class Offer : BaseEntity
{
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int ColorId { get; set; }
    public DateTime StartDate { get; set; }
    [ForeignKey(nameof(ColorId))] public Color? Color { get; set; }
    public int? ProductId { get; set; }
    public double OfferAmount { get; set; }
}