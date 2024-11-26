using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.IndexPage;

namespace Domain.Entity.Product;

public class Brand : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? SubCategoryId { get; set; }
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
    public int OnClick { get; set; }
    [ForeignKey("SubCategoryId")] public SubCategory? SubCategory { get; set; }
}