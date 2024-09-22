using Application.Dtos.Client;
using AutoMapper;
using Domain.Entity.IndexPage;

namespace Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Translation
        CreateMap<FooterLink, FooterDto>()
            .ReverseMap();
        CreateMap<BrandLanding, BrandLandingDto>()
            .ReverseMap();
    }
}