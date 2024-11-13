using Domain.Common;

namespace Domain.Entity.Factor;

public class BankTransaction : BaseEntity
{
    public int FactorId { get; set; }
    public long SaleReferenceId { get; set; }
    public string? StatusPayment { get; set; }
    public bool PaymentFinished { get; set; }
    public long Amount { get; set; }
    public string BankName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime BuyDatetime { get; set; }
}