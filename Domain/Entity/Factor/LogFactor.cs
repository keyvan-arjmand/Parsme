using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Factor;

public  class LogFactor:BaseEntity
{
    public int FactorId { get; set; }
    [ForeignKey("FactorId")] public Factor? Factor { get; set; } 
    public int UserId { get; set; }
    [ForeignKey("UserId")] public User.User? User { get; set; }
    public string Desc { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
}