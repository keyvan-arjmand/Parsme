using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ReturnedStatus
{
    [Display(Name = "ارسال شده")]
    Accepted = 0,
    [Display(Name = "رد شده")]
    Rejected = 1,
    [Display(Name = "در حال پردازش")]
    Pending=2,
    [Display(Name = "مرجوعی")]
    Returned = 3
}