using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class Brand:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public int? SubCategoryId { get; set; }
    [ForeignKey("SubCategoryId")] public SubCategory? SubCategory { get; set; }
    public int? BrandDetailId { get; set; }
    [ForeignKey("BrandDetailId")] public BrandDetail? BrandDetail { get; set; }
 
}