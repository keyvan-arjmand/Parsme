using Domain.Common;

namespace Domain.Entity.IndexPage;

public class SearchResult:BaseEntity
{
    public string Value { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}