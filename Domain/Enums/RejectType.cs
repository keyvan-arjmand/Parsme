using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum RejectType
{
    [Display(Name = "عدم انطباق با توضیحات")]
    rj=0,
    [Display(Name = "عدم رعایت قوانین مرجوعی، خارج از بازه 7 روزه")]
    adam=1
}