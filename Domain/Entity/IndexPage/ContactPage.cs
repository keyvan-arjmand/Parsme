using Domain.Common;

namespace Domain.Entity.IndexPage;

public class ContactPage:BaseEntity
{
    public string Desc { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
}