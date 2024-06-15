using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ImageGallery:BaseEntity
{
    public string ImageUri { get; set; } = string.Empty;
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; } = new();
}