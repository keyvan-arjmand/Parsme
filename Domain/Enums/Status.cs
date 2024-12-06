using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Status
{
    [Display(Name = "در حال پردازش")] Pending = 2,
    [Display(Name = "آماده ارسال")] ReadyToSend =7,
    [Display(Name = "ارسال شده")] Accepted = 0,
    [Display(Name = "رد شده")] Rejected = 1,
    [Display(Name = "آماده دریافت حضوری")] ReadyToGet =8,
    [Display(Name = "در انتظار پرداخت")] PendingForPayment =6,
    [Display(Name = "پرداخت ناموفق")] Field = 5,
    [Display(Name = "مرجوعی")] Returned = 3,
    [Display(Name = "لغو سفارش توسط خریدار")] Canceled = 3,
}