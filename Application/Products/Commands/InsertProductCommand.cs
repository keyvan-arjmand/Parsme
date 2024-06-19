using Domain.Enums;
using MediatR;

namespace Application.Products.Commands;

public class InsertProductCommand:IRequest
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
    public int SubCategoryId { get; set; }
    public List<string> Images { get; set; } = default!;
    public List<ProductDetail> ProductDetails { get; set; }= default!;
    public List<ProductColor> ProductColors { get; set; }= default!;
    public ProductStatus ProductStatus { get; set; }
    public bool IsActive { get; set; }

}
public abstract class ProductDetail
{
    public int CatDetailId { get; set; }
    public string Value { get; set; } = string.Empty;
}
public abstract class ProductColor
{
    public double Price { get; set; }
    public int ColorId { get; set; }
    public int GuaranteeId { get; set; }
    public int Inventory { get; set; }
    public bool IsOffer { get; set; }
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public double OfferAmount { get; set; }
}