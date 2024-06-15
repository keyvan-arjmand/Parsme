using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Products;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public IFormFile ImageBanner { get; set; } = default!;
    public IFormFile ImageThumb { get; set; } = default;
    public IFormFile Image { get; set; } = default;
    public double Weight { get; set; }//وزن 
    public double Wages { get; set; }//اجرت 
    public long Inventory { get; set; }
    public long CategoryId { get; set; }
}