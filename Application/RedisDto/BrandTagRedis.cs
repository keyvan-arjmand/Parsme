namespace Application.RedisDto;

public class BrandTagRedis
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public bool IsClick { get; set; }
}