using Application.Interfaces;
using Domain.Entity.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands;

public class InsertProductCommandHandler:IRequestHandler<InsertProductCommand>
{
    private readonly IUnitOfWork _work;

    public InsertProductCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await _work.GenericRepository<Brand>().Table
            .FirstOrDefaultAsync(x => x.Id == request.BrandId, cancellationToken);
        if (brand == null) throw new Exception();
        var subCat = await _work.GenericRepository<SubCategory>().Table
            .FirstOrDefaultAsync(x => x.Id == request.SubCategoryId, cancellationToken);
        if (subCat == null) throw new Exception();
        var product = new Product
        {
            Code = request.Code,
            Title = request.Title,
            InsertDate = DateTime.Now,
            IsActive = request.IsActive,
            ProductStatus = request.ProductStatus,
            SubCategoryId = subCat.Id,
            BrandId = brand.Id,
            Detail = request.Detail,
            DiscountAmount = request.DiscountAmount,
            FullDesc = request.FullDesc,
            MetaKeyword = request.MetaKeyword,
            PersianTitle = request.PersianTitle,
            MetaDesc = request.MetaDesc,
            ProductGift = request.ProductGift,
        };
        var productGallery = request.Images.Select(x => new ImageGallery
        {
            ProductId = product.Id,
            ImageUri = x
        }).ToList();
        var prodDetail = request.ProductDetails.Select(x => new Domain.Entity.Product.ProductDetail
        {
            ProductId = product.Id,
            Value = x.Value,
            CategoryDetailId = x.CatDetailId
        });
        var productColor = request.ProductColors.Select(x => new Domain.Entity.Product.ProductColor
        {
            ProductId = product.Id,
            Priority = 1,
            Days = x.Days,
            OfferAmount = x.OfferAmount,
            IsOffer = x.IsOffer,
            GuaranteeId = x.GuaranteeId,
            ColorId = x.ColorId,
            Price = x.Price,
            Minutes = x.Minutes,
            Inventory = x.Inventory,
            Hours = x.Hours,
        });
    }
}