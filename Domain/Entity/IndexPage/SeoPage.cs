using Domain.Common;

namespace Domain.Entity.IndexPage;

public class SeoPage:BaseEntity
{
    public string SeoIndexTitle { get; set; } = string.Empty;
    public string SeoIndexDesc { get; set; } = string.Empty;
    public string SeoIndexCanonical { get; set; } = string.Empty;
    public string TopBanner { get; set; } = string.Empty;
    public string TopBannerHref { get; set; } = string.Empty;
}