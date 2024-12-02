using Domain.Common;

namespace Domain.Entity.IndexPage;

public class SeoPage : BaseEntity
{
    public string SeoIndexTitle { get; set; } = string.Empty;
    public string SeoIndexDesc { get; set; } = string.Empty;
    public string SeoIndexCanonical { get; set; } = string.Empty;
    public string TopBanner { get; set; } = string.Empty;
    public bool ShowTopBanner { get; set; }
    public string TopBannerHref { get; set; } = string.Empty;
    public string NavNameComp { get; set; } = string.Empty;
    public string HeaderNumber { get; set; } = string.Empty;
    public string HeaderLogo { get; set; } = string.Empty;
    public string EmailComp { get; set; } = string.Empty;
    
    
    public string ProductTitle { get; set; } = string.Empty;
    public string ProductTitle2 { get; set; } = string.Empty;
    public string ProductTitle3 { get; set; } = string.Empty;
    public string ProductTitle4 { get; set; } = string.Empty;
}