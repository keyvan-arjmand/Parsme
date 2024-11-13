using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Status
{
    [Display(Name = "ارسال شده")] Accepted = 0,
    [Display(Name = "لغو شده")] Rejected = 1,
    [Display(Name = "در حال پردازش")] Pending = 2,
    [Display(Name = "مرجوعی")] Returned = 3,
    [Display(Name = "درحال آماده سازی")] Making = 3,
    [Display(Name = "دریافت شده")] Recive = 3,
    [Display(Name = "پرداخت ناموفق")] Field = 3,
    [Display(Name = "در انتظار پرداخت")] PendingForPayment = 4,
}