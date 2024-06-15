namespace Application.Dtos.Factor;

public class ProductFactorDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public double Weight { get; set; } //وزن 
    public double Wages { get; set; } //اجرت 
    public long Inventory { get; set; }
    public string GenderType { get; set; } = string.Empty; //نوع جنس
    public string CategoryName { get; set; } = string.Empty;
    public string ImageBanner { get; set; } = string.Empty;
    public string ImageThumb { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}