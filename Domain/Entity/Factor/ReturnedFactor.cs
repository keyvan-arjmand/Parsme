using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entity.Factor;

public  class ReturnedFactor:BaseEntity
{
    public ReturnedType ReturnedType { get; set; }
    public ReturnedStatus ReturnedStatus { get; set; }
    public int? FactorId { get; set; }
    [ForeignKey("FactorId")] public Factor? Factor { get; set; } 
    public string Desc { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
}