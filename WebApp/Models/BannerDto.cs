namespace WebApp.Models;

public class BannerDto
{
    public int Id { get; set; }
    public IFormFile LargeSideBanner { get; set; } 
    public string LargeSideBannerHref { get; set; } = string.Empty;
    
    public IFormFile SmallSideBanner { get; set; }
    public string SmallSideBannerHref { get; set; } = string.Empty;
    
    public IFormFile SmallBannerMiddle1 { get; set; }
    public string SmallBannerMiddle1Href { get; set; } = string.Empty;
    public IFormFile SmallBannerMiddle2 { get; set; }
    public string SmallBannerMiddle2Href { get; set; } = string.Empty;
    public IFormFile SmallBannerMiddle3 { get; set; }
    public string SmallBannerMiddle3Href { get; set; } = string.Empty;
    public IFormFile SmallBannerMiddle4 { get; set; } 
    public string SmallBannerMiddle4Href { get; set; } = string.Empty;
    
    public IFormFile BigBannerMiddle1 { get; set; } 
    public string BigBannerMiddle1Href { get; set; } = string.Empty;
    public IFormFile BigBannerMiddle2 { get; set; }
    public string BigBannerMiddle2Href { get; set; } = string.Empty;
    
    public string SliderTitle { get; set; } = string.Empty;
    public IFormFile SliderImage { get; set; }
    public string SliderHref { get; set; } = string.Empty;
    
    public string SliderTitle1 { get; set; } = string.Empty;
    public IFormFile SliderImage1 { get; set; } 
    public string SliderHref1 { get; set; } = string.Empty;

    public string SliderTitle2 { get; set; } = string.Empty;
    public IFormFile SliderImage2 { get; set; }
    public string SliderHref2 { get; set; } = string.Empty;
}