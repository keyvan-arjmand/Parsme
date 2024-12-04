namespace Application.RedisDto;

public class CategoryRedis
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public int? MainCategoryId { get; set; }
    //public MainCategoryRedis? MainCategory { get; set; }
    public List<SubCategoryRedis> SubCategories { get; set; } = default!;
    public bool IsActive { get; set; }
}