using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Factor;

public class LogReturnedFactor:BaseEntity
{
    public int? ReturnedFactorId { get; set; }
    [ForeignKey("ReturnedFactorId")] public ReturnedFactor? Factor { get; set; } 
    public int? UserId { get; set; }
    [ForeignKey("UserId")] public User.User? User { get; set; }
    public string Desc { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
}