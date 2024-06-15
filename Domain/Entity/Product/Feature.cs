using Domain.Common;

namespace Domain.Entity.Product;

public class Feature:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int Priority { get; set; }
}