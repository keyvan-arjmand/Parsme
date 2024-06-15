using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.User;

public class City : BaseEntity
{
    public int StateId { get; set; }
    [ForeignKey("StateId")] public State State { get; set; } = new();

    public string Name { get; set; } = string.Empty;


}