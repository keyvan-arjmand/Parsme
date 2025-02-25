namespace Admin.Ui.Models.Dtos;

public class BrandTagDto
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public IFormFile LogoUriFile { get; set; } = null!;
    public bool IsClick { get; set; }
}