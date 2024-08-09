using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Client;

public class SaleService
{
    public int Id { get; set; }
    public string Desc1 { get; set; } = string.Empty;
    public IFormFile Desc1Logo { get; set; }
    public string Desc2 { get; set; } = string.Empty;
    public IFormFile Desc2Logo { get; set; }
    public string Desc3 { get; set; } = string.Empty;
    public IFormFile Desc3Logo { get; set; }
    public string Desc4 { get; set; } = string.Empty;
    public IFormFile Desc4Logo { get; set; }
    public string Desc5 { get; set; } = string.Empty;
    public IFormFile Desc5Logo { get; set; }
}