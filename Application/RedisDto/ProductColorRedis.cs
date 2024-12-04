namespace Application.RedisDto;

public class ProductColorRedis
{
    public int Id { get; set; }

    public double Price { get; set; }//RIT
    public int Priority { get; set; }
    public int ProductId { get; set; }
    //public ProductRedis? Product { get; set; } 
    public int ColorId { get; set; }
    public ColorRedis? Color { get; set; } 
    public int GuaranteeId { get; set; }
    public GuaranteeRedis? Guarantee { get; set; } 
    public int Inventory { get; set; }
}