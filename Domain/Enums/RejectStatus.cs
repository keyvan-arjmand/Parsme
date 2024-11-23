using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum RejectStatus
{
    [Display(Name = "هیچکدام")]  None=0,
    [Display(Name = "اتمام موجودی")] Address = 1,
    [Display(Name = "مشخص نبودن آدرس")] Inventory = 2,
}