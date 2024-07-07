using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class UserFav:BaseEntity
{
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product? Product { get; set; }
    public int UserId { get; set; }
}