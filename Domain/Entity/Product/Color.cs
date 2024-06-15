using Domain.Common;

namespace Domain.Entity.Product;

public class Color:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
}