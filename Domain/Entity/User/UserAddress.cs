using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.User;

public class UserAddress:BaseEntity
{
    public int UserId { get; set; }
    public int CityId { get; set; }
    [ForeignKey("CityId")] public City City { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}