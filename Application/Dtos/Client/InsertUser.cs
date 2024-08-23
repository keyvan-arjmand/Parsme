using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Client;

public class InsertUser
{
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IFormFile Image { get; set; } 
    public string PhoneNumber { get; set; } = string.Empty;
    public int CityId { get; set; }
    public List<string> Role { get; set; } 
    public string Password { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Sheba { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public string? EconomicNumber { get; set; }
    public string? OrganizationName { get; set; }
    public string? NationalId { get; set; }
    public string? PostCode { get; set; }
    public string? OrganizationNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Adderss { get; set; }
    
}