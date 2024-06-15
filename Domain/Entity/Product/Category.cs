using Domain.Common;

namespace Domain.Entity.Product;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public ICollection<SubCategory> SubCategories { get; set; } = default!;
    public bool IsActive { get; set; }
}