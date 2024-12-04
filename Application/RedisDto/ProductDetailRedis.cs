namespace Application.RedisDto;

public class ProductDetailRedis
{
    public int Id { get; set; }

    public int CategoryDetailId { get; set; }
    public CategoryDetailRedis? CategoryDetail { get; set; }
    public int ProductId { get; set; }
    public int Priority { get; set; }

    public string? Value { get; set; }
}