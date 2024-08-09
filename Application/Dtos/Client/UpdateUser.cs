namespace Application.Dtos.Client;

public class UpdateUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int CityId { get; set; }
    public List<string> Role { get; set; } 
    public string Password { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Sheba { get; set; } = string.Empty;
}