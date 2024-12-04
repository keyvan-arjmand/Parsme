using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Application.RedisDto;

public class ProductRedis
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string PersianTitle { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string MetaKeyword { get; set; } = string.Empty;
    public string FullDesc { get; set; } = string.Empty;
    public string ImageUri { get; set; } = string.Empty;
    public string? ProductGift { get; set; }
    public string Strengths { get; set; } = string.Empty;
    public string WeakPoints { get; set; } = string.Empty;
    public string UnicCode { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }
    public double InterestRate { get; set; }

    public int BrandId { get; set; }
    public BrandRedis Brand { get; set; } = default!;
    public int? BrandTagId { get; set; }
    public BrandTagRedis? BrandTag { get; set; }
    public int? OfferId { get; set; }
    public OfferRedis? Offer { get; set; }
    public int SubCategoryId { get; set; }
    public SubCategoryRedis SubCategory { get; set; } = default!;
    public List<ImageGalleryRedis> ProductImages { get; set; } = default!;
    public List<ProductDetailRedis> ProductDetails { get; set; } = default!;
    public List<ProductColorRedis> ProductColors { get; set; } = default!;

    public ProductStatus ProductStatus { get; set; }
    public bool IsActive { get; set; }
    public bool IsOffer { get; set; }
    public bool IsShowIndex { get; set; }
    public bool MomentaryOffer { get; set; }
    public int OnClick { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime UpdateTime { get; set; }

    public string? SeoTitle { get; set; }
    public string? SeoDesc { get; set; }
    public string? SeoCanonical { get; set; }
 
}