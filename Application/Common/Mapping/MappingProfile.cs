using Application.Dtos.Client;
using Application.RedisDto;
using AutoMapper;
using Domain.Entity.IndexPage;
using Domain.Entity.Product;

namespace Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Translation
        CreateMap<FooterLink, FooterDto>()
            .ReverseMap();
        CreateMap<Category, SubCategoryRedis.CategoryInternal>()
            .ReverseMap();
        CreateMap<BrandLanding, BrandLandingDto>()
            .ReverseMap();
        CreateMap<GuaranteeRedis, Guarantee>()
            .ReverseMap();
        CreateMap<ImageGalleryRedis, ImageGallery>()
            .ReverseMap();
        CreateMap<ColorRedis, Color>()
            .ReverseMap();
        CreateMap<BrandRedis, Brand>()
            .ReverseMap();

        CreateMap<BrandTagRedis, BrandTag>()
            .ReverseMap();
        CreateMap<FeatureRedis, Feature>()
            .ReverseMap();
        CreateMap<CategoryDetailRedis, CategoryDetail>()
            .ReverseMap();
        CreateMap<OfferRedis, Offer>()
            .ReverseMap();
        CreateMap<ProductColorRedis, ProductColor>()
            .ReverseMap();

        CreateMap<Banner, BannerRedis>()
            .ReverseMap();
        CreateMap<MainCategory, SubCategoryRedis.MainCategoryInternal>()
            .ReverseMap();
        CreateMap<SearchResult, SearchResultRedis>()
            .ReverseMap();
        CreateMap<SeoPage, SeoPageRedis>()
            .ReverseMap();
        CreateMap<FooterLink, FooterLinkRedis>()
            .ReverseMap();

        CreateMap<ProductDetailRedis, ProductDetail>()
            .ReverseMap();


        CreateMap<SubCategoryRedis, SubCategory>()
            .ReverseMap();
        CreateMap<SubCategoryDetailRedis, SubCategoryDetail>()
            .ReverseMap();
        CreateMap<CategoryRedis, Category>()
            .ReverseMap();
        CreateMap<MainCategoryRedis, MainCategory>()
            .ReverseMap();
        CreateMap<ProductRedis, Product>()
            .ReverseMap();
    }
}