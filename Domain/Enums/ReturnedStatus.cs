using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ReturnedStatus
{
    [Display(Name = "مرجوع شده")]
    Accepted = 0,
    [Display(Name = "رد درخواست از طرف پارس می")]
    Rejected = 1,
    [Display(Name = " در حال بررسی فنی")]
    Pending=2,
    [Display(Name = "دریافت کالا")]
    Returned = 3,
   
}