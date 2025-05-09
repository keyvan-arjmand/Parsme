﻿using Application.Dtos;

namespace Application.Products.Commands;

public class Root
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PersianTitle { get; set; }
    public List<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    public List<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    public string Detail { get; set; }
    public string MetaDesc { get; set; }
    public string MetaKeyword { get; set; }
    public string FullDesc { get; set; }
    public string ImageUri { get; set; }
    public string ProductGift { get; set; }
    public string DiscountAmount { get; set; }
    public string BrandId { get; set; }
    public string BrandTagId { get; set; }
    public string UnicCode { get; set; } = string.Empty;
    public string SubCategoryId { get; set; }
    public string ProductStatus { get; set; }
    public bool IsActive { get; set; }
    public bool IsShowIndex { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public bool IsOffer { get; set; }
    public Offer Offer { get; set; }
    public string Strengths { get; set; } = string.Empty;
    public string WeakPoints { get; set; } = string.Empty;
    public bool MomentaryOffer { get; set; }

    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
    public double InterestRate { get; set; }
}

public class Offer
{
    public string ColorId { get; set; }
    public string OfferAmount { get; set; }
    public string Hours { get; set; }
    public string Minutes { get; set; }
    public DateTime Time { get; set; }
}

public class ProductColor
{
    public int Id { get; set; }
    public string Gu { get; set; }
    public string ColorInv { get; set; }
    public string ColorId { get; set; }
    public string ColorPrice { get; set; }
    public List<ColorGuarantee> ColorGuarantees { get; set; }
}

public class ColorGuarantee
{
    public int Id { get; set; }
    public string Gu { get; set; }
    public string ColorPrice { get; set; }
    public string ColorInv { get; set; }
}
public class ProductDetail
{
    public int Id { get; set; }
    public string DetailId { get; set; }
    public string DetailName { get; set; }
}