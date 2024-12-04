namespace Application.RedisDto;

public class OfferRedis
{
    public int Id { get; set; }

    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int ColorId { get; set; }
    public DateTime StartDate { get; set; }
    public ColorRedis? Color { get; set; }
    public int? ProductId { get; set; }
    public double OfferAmount { get; set; }
}