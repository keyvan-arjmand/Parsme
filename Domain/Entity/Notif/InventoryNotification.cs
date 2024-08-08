using Domain.Common;

namespace Domain.Entity.Notif;

public class InventoryNotification:BaseEntity
{
    public int UserId { get; set; }
    public int ProductColorId { get; set; }
    public DateTime InsertDate { get; set; }
}