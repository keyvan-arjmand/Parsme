using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class SubCategoryDetail:BaseEntity
{
    public int SubCategoryId { get; set; }
    [ForeignKey("SubCategoryId")] public SubCategory? SubCategory { get; set; }
    public int CategoryDetailId { get; set; }
}