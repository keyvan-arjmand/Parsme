using System.Runtime;
using Domain.Common;

namespace Domain.Entity.IndexPage;

public class Banner:BaseEntity
{
    public string LargeSideBanner { get; set; } = string.Empty;
    public string LargeSideBannerHref { get; set; } = string.Empty;
    
    public string SmallSideBanner { get; set; } = string.Empty;
    public string SmallSideBannerHref { get; set; } = string.Empty;
    
    public string SmallBannerMiddle1 { get; set; } = string.Empty;
    public string SmallBannerMiddle1Href { get; set; } = string.Empty;
    public string SmallBannerMiddle2 { get; set; } = string.Empty;
    public string SmallBannerMiddle2Href { get; set; } = string.Empty;
    public string SmallBannerMiddle3 { get; set; } = string.Empty;
    public string SmallBannerMiddle3Href { get; set; } = string.Empty;
    public string SmallBannerMiddle4 { get; set; } = string.Empty;
    public string SmallBannerMiddle4Href { get; set; } = string.Empty;
    
    public string BigBannerMiddle1 { get; set; } = string.Empty;
    public string BigBannerMiddle1Href { get; set; } = string.Empty;
    public string BigBannerMiddle2 { get; set; } = string.Empty;
    public string BigBannerMiddle2Href { get; set; } = string.Empty;
}