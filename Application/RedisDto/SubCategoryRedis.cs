namespace Application.RedisDto;

public class SubCategoryRedis
{
    public class CategoryInternal
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string LogoUri { get; set; } = string.Empty;
        public int? MainCategoryId { get; set; }
        public bool IsActive { get; set; }
        public MainCategoryInternal? MainCategory { get; set; }
    }  
    public class MainCategoryInternal
    {
        public string Name { get; set; } = string.Empty;
        public string LogoUri { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public List<CategoryDetailRedis> CategoryDetails { get; private set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public string SeoTitle { get; set; }
    public string SeoDesc { get; set; }
    public string SeoCanonical { get; set; }
    public CategoryInternal? Category { get; set; }
    public List<BrandRedis> Brands { get; set; }
}