using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entity.Product;

public class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string PersianTitle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string MetaDesc { get; set; } = string.Empty;
    public string MetaKeyword { get; set; } = string.Empty;
    public string FullDesc { get; set; } = string.Empty;
    public string ImageUri { get; set; } = string.Empty;
    public string ProductGift { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }

    public int BrandId { get; set; }
    [ForeignKey(nameof(BrandId))] public Brand Brand { get; set; } = default!;
    public int? OfferId { get; set; }
    [ForeignKey(nameof(OfferId))] public Offer? Offer { get; set; }
    public int SubCategoryId { get; set; }
    [ForeignKey(nameof(SubCategoryId))] public SubCategory SubCategory { get; set; } = default!;
    public ICollection<ImageGallery> ProductImages { get; set; } = default!;
    public ICollection<ProductDetail> ProductDetails { get; set; } = default!;
    public ICollection<ProductColor> ProductColors { get; set; } = default!;

    public ProductStatus ProductStatus { get; set; }
    public bool IsActive { get; set; }
    public bool IsOffer { get; set; }


    public DateTime InsertDate { get; set; }
}