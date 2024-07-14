using Domain.Common;

namespace Domain.Entity.User;

public class PostMethod:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public double Price { get; set; }
}