namespace Infrastructure.Repositories;

public class AppSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Encryptkey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int NotBeforeMinutes { get; set; }
    public int ExpirationYear { get; set; }
}