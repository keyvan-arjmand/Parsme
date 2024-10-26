using Domain.Entity.Product;

namespace Application.Dtos.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string PersianTitle { get; set; } = string.Empty;
    public string ImageUri { get; set; } = string.Empty;
    public ICollection<ProductColor> ProductColors { get; set; } = default!;
}