namespace Application.RedisDto;

public class SubCategoryDetailRedis
{
    public int Id { get; set; }

    public int SubCategoryId { get; set; }
    public SubCategoryRedis? SubCategory { get; set; }
    public int CategoryDetailId { get; set; }
}