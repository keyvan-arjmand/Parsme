using Domain.Common;

namespace Domain.Entity.Product;

public class DiscountCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public double Amount { get; set; }
    public int Count { get; set; }
    public int FirstCount { get; set; }
    public bool IsActive { get; set; }
}