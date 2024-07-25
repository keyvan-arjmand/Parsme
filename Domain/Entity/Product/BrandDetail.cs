using Domain.Common;

namespace Domain.Entity.Product;

public class BrandDetail:BaseEntity
{
    public int BrandId { get; set; }
    public string SliderImage { get; set; } = string.Empty;
    public string SliderHref { get; set; } = string.Empty;
    
    public string SliderImage1 { get; set; } = string.Empty;
    public string SliderHref1 { get; set; } = string.Empty;
    
    public string SliderImage2 { get; set; } = string.Empty;
    public string SliderHref2 { get; set; } = string.Empty;
    
    public string FirstBannerImage { get; set; } = string.Empty;
    public string FirstBannerHref { get; set; } = string.Empty;
    public string SecBannerImage { get; set; } = string.Empty;
    public string SecBannerHref { get; set; } = string.Empty;
}