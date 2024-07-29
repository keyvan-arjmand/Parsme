using Domain.Common;

namespace Domain.Entity.IndexPage;

public class AboutUsPage:BaseEntity
{
    public string Head { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}