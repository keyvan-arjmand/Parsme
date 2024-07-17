namespace WebApp.Models;

public class InsertUser
{
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IFormFile Image { get; set; } 
    public string PhoneNumber { get; set; } = string.Empty;
    public int CityId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Sheba { get; set; } = string.Empty;
}