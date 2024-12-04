namespace Application.RedisDto;

public class ImageGalleryRedis
{
    public int Id { get; set; }

    public string ImageUri { get; set; } = string.Empty;
    public int ProductId { get; set; }
}