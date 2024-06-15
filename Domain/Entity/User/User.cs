using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity.Product;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity.User;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public int CityId { get; set; }
    [ForeignKey("CityId")] public City City { get; set; } = default!;
    public string ConfirmCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; } = DateTime.Now;
    public DateTime ConfirmCodeExpireTime { get; set; }
    public string Address { get; set; } = string.Empty;

    
}