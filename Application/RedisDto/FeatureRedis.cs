namespace Application.RedisDto;

public class FeatureRedis
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}