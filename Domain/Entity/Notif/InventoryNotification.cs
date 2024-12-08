using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Notif;

public class InventoryNotification:BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int ProductColorId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product.Product Product { get; set; } = default!;
    public DateTime InsertDate { get; set; }
    public bool IsRemind { get; set; }
}