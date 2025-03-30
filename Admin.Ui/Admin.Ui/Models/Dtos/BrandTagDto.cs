using Microsoft.AspNetCore.Components.Forms;

namespace Admin.Ui.Models.Dtos;

public class BrandTagDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public IBrowserFile LogoUriFile { get; set; } = null!;
    public bool IsClick { get; set; }
}