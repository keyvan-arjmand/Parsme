using Domain.Common;

namespace Domain.Entity.Product;

public class MainCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Category> Categories { get; private set; } = default!;
}