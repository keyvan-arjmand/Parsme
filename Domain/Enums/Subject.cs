using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Subject
{
    [Display(Name = "پیشنهاد")]
    Proposal=0,
    [Display(Name = "انتقاد یا شکایات")]
    Criticism=1,
    [Display(Name = "پیگیری سفارش")]
    FollowUp=2,
    [Display(Name = "خدمات پس از فروش")]
    Services=3,
    [Display(Name = "استعلام گارانتی")]
    Inquiry=4,
    [Display(Name = "مدیریت")]
    Management=5
}