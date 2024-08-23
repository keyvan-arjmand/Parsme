using Domain.Entity.User;
using Domain.Enums;

namespace Application.Dtos.Client;

public class UserDto
{
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public int CityId { get; set; }
    public City City { get; set; } = default!;
    public string ConfirmCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Sheba { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; } = DateTime.Now;
    public DateTime ConfirmCodeExpireTime { get; set; }
    public List<string> Roles { get; set; } = default!;
    public AccountType AccountType { get; set; }
    public string? EconomicNumber { get; set; }
    public string? OrganizationName { get; set; }
    public string? NationalId { get; set; }
    public string? PostCode { get; set; }
    public string? RegistrationNumber { get; set; }
    
    public string? OrganizationNumber { get; set; }
    public string? Adderss { get; set; }
}