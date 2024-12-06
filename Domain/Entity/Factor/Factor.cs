using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.Factor.Product;
using Domain.Entity.User;
using Domain.Enums;

namespace Domain.Entity.Factor;

public class Factor:BaseEntity
{
    public int? UserAddressId { get; set; }
    [ForeignKey("UserAddressId")] public UserAddress? UserAddress { get; set; } 
    public int? UserId { get; set; }
    [ForeignKey("UserId")] public User.User? User { get; set; }
    public int? PostMethodId { get; set; }
    [ForeignKey("PostMethodId")] public PostMethod? PostMethod { get; set; }
    public string Desc { get; set; } = string.Empty;
    public string FactorCode { get; set; } = string.Empty;
    public string DiscountCode { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }//discount code
    public double Amount { get; set; }//pure price
    public double AmountPrice { get; set; }//offer
    public DateTime InsertDate { get; set; }
    public bool IsReturned { get; set; }
    public Status Status { get; set; }
    public RejectStatus RejectStatus { get; set; } 
    public ICollection<FactorProduct> Products { get; set; } = default!;
    public ICollection<LogFactor> Logs { get; set; } = default!;
    public string? ReferenceNumber { get; set; }
    public long SaleReferenceId { get; set; }
    public string? StatusPayment { get; set; }
    public bool IsLegal { get; set; }
    public AccountType AccountType { get; set; }
    
    public string? RefPostUrl { get; set; }
    public string? RecipientName { get; set; }
    public string? EconomicNumber { get; set; }//شماره اقتصادی
    public string? OrganizationName { get; set; }//نام سازمان
    public string? NationalId { get; set; }//شناسه ملی
    public string? PostCode { get; set; }
        public string? OrganizationNumber { get; set; }//شماره سازمان
    public string? RegistrationNumber { get; set; }//شماره ثبت
    public string? Adders { get; set; }//
}