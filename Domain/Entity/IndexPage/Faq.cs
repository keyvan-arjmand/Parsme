using Domain.Common;

namespace Domain.Entity.IndexPage;

public class Faq:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
}