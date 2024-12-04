namespace Application.RedisDto;

public class MainCategoryRedis
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<CategoryRedis> Categories { get; set; }
}