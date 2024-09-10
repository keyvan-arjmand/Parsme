using Application.Common.Utilities;
using Application.Interfaces;
using Domain.Entity.Product;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vit.Extensions.Json_Extensions;

namespace Application.Products.Commands;

public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand>
{
    private readonly IUnitOfWork _work;

    public InsertProductCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await _work.GenericRepository<Brand>().Table
            .FirstOrDefaultAsync(x => x.Id == request.Product.BrandId.ToInt(), cancellationToken);
        if (brand == null) throw new Exception();
        var subCat = await _work.GenericRepository<SubCategory>().Table
            .FirstOrDefaultAsync(x => x.Id == request.Product.SubCategoryId.ToInt(), cancellationToken);
        if (subCat == null) throw new Exception();
        var product = new Product
        {
            Code = string.Empty,
            Title = request.Product.Title!,
            InsertDate = DateTime.Now,
            IsActive = request.Product.IsActive,
            ProductStatus = (ProductStatus)request.Product.ProductStatus.ToInt(),
            SubCategoryId = subCat.Id,
            BrandId = brand.Id,
            Detail = request.Product.Detail!,
            DiscountAmount = request.Product.DiscountAmount.ToDouble(),
            FullDesc = request.Product.FullDesc!,
            MetaKeyword = request.Product.MetaKeyword!,
            PersianTitle = request.Product.PersianTitle!,
            ProductGift = request.Product.ProductGift!,
            ImageUri = request.Product.ImageUri,
            IsOffer = request.Product.IsOffer,
            Strengths = request.Product.Strengths,
            WeakPoints = request.Product.WeakPoints,
            MomentaryOffer = request.Product.MomentaryOffer,
            IsShowIndex = request.Product.IsShowIndex,
            SeoCanonical = request.Product.SeoCanonical,
            InterestRate = request.Product.InterestRate,
            SeoDesc = request.Product.SeoDesc,
            SeoTitle = request.Product.SeoTitle,
            UnicCode = request.Product.UnicCode
        };
        await _work.GenericRepository<Product>().AddAsync(product, CancellationToken.None);
        var productGallery = request.Product.Images.Select(x => new ImageGallery
        {
            ProductId = product.Id,
            ImageUri = x
        }).ToList();
        foreach (var i in productGallery)
        {
            await _work.GenericRepository<Domain.Entity.Product.ImageGallery>()
                .AddAsync(i, CancellationToken.None); 
        }
     
        var prodDetail = request.Product.ProductDetails.Select(x => new Domain.Entity.Product.ProductDetail
        {
            ProductId = product.Id,
            Value = x.DetailName!,
            CategoryDetailId = x.DetailId.ToInt()
        });
        foreach (var i in prodDetail)
        {
            await _work.GenericRepository<Domain.Entity.Product.ProductDetail>()
                .AddAsync(i, CancellationToken.None); 
        }
     
        var productColor = request.Product.ProductColors.Select(x => new Domain.Entity.Product.ProductColor
        {
            ProductId = product.Id,
            Priority = 1,
            GuaranteeId = x.Gu.ToInt(),
            ColorId = x.ColorId.ToInt(),
            Price = x.ColorPrice.ToDouble(),
            Inventory = x.ColorInv.ToInt(),
        });
        foreach (var i in productColor)
        {
            await _work.GenericRepository<Domain.Entity.Product.ProductColor>()
                .AddAsync(i, CancellationToken.None); 
        }
   
        if (request.Product.IsOffer)
        {
            var offer = new Domain.Entity.Product.Offer
            {
                Days = request.Product.Offer.Days.ToInt(),
                ColorId = request.Product.Offer.ColorId.ToInt(),
                Hours = request.Product.Offer.Hours.ToInt(),
                Minutes = request.Product.Offer.Minutes.ToInt(),
                OfferAmount = request.Product.Offer.OfferAmount.ToDouble(),
                ProductId = product.Id,
                StartDate = request.Product.Offer.Time
            };
            await _work.GenericRepository<Domain.Entity.Product.Offer>().AddAsync(offer, CancellationToken.None);
            product.OfferId = offer.Id;
            await _work.GenericRepository<Product>().UpdateAsync(product, CancellationToken.None);
        }
    }
}