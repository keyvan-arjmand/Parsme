using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum AccountType
{
    [Display(Name = "حقیقی")]
    Genuine=0,
    [Display(Name = "حقوقی")]
    Legal=1
}