using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ReturnedType
{
    [Display(Name = "عدم تطابق رنگ")]
    Colormatching=0,
    [Display(Name = "پلمپ نبودن دستگاه")]
    sealing=1,
    [Display(Name = "آسیب فیزیکی")]
    damage=2,
    [Display(Name = "عدم تطابق مشخصات فنی")]
    Mismatch=3,
}