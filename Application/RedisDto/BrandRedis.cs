namespace Application.RedisDto;

public class BrandRedis
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string LogoUri { get; set; }
    public bool IsActive { get; set; }
    public int SubCategoryId { get; set; }
    public string SeoTitle { get; set; }
    public string SeoDesc { get; set; }
    public string SeoCanonical { get; set; }
    public int OnClick { get; set; }
}