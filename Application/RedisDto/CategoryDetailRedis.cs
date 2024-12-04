using Domain.Enums;

namespace Application.RedisDto;

public class CategoryDetailRedis
{
    public int Id { get; set; }

    public int FeatureId { get; set; }
    public FeatureRedis? Feature { get; set; }
    public string Title { get; set; } = string.Empty;
    public DataType DataType { get; set; }
    public string Option { get; set; } = string.Empty;
    public bool ShowInSearch { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    //public ICollection<SubCategoryDetailRedis> SubCategoryDetails { get; set; } = default!;
}