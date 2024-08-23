using Domain.Common;

namespace Domain.Entity.IndexPage;

public class AboutUsPage:BaseEntity
{
    public string Head { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
}