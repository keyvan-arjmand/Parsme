using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ReturnedStatus
{
    [Display(Name = "عودت کالا به کاربر")]
    Accepted = 0,
    [Display(Name = "لغو درخواست از طرف پارس می")]
    Rejected = 1,
    [Display(Name = " در حال بررسی فنی")]
    Pending=2,
    [Display(Name = "مرجوعی")]
    Returned = 3,
    [Display(Name = " ارسال کالا توسط کاربر")]
    Sending = 4
    ,
    [Display(Name = "لغو درخواست از طرف کاربر")]
    RejectedByUser = 4
}