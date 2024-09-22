using Domain.Common;

namespace Domain.Entity.IndexPage;

public class FaqCat:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public ICollection<Faq> Faqs { get; set; } = default!;
}