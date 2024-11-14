using Domain.Entity.Product;
using Domain.Enums;

namespace WebApp.Models;

public class BasketProduct
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PersianTitle { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string MetaDesc { get; set; } = string.Empty;
    public string MetaKeyword { get; set; } = string.Empty;
    public string FullDesc { get; set; } = string.Empty;
    public string ImageUri { get; set; } = string.Empty;
    public string ProductGift { get; set; } = string.Empty;
    public string Strengths { get; set; } = string.Empty;
    public string WeakPoints { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }
    public double InterestRate { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = default!;
    public int? OfferId { get; set; }
     public Offer? Offer { get; set; }
    public int SubCategoryId { get; set; }
    public int Count { get; set; }
    public SubCategory SubCategory { get; set; } = default!;
    public List<ImageGallery> ProductImages { get; set; } = default!;
    public List<ProductDetail> ProductDetails { get; set; } = default!;
    public List<ProductColor> ProductColors { get; set; } = default!;

    public ProductStatus ProductStatus { get; set; }
    public bool IsActive { get; set; }
    public bool IsOffer { get; set; }
    public bool IsShowIndex { get; set; }
    public bool MomentaryOffer { get; set; }

    public DateTime InsertDate { get; set; }
}