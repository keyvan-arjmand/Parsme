using AutoMapper;
using Domain.Common;

namespace Application.Common.Mapping;

public static class MapperExtensions
{
    private static IMapper? _mapper;

    public static void Configure(IMapper? mapper)
    {
        _mapper = mapper;
    }


    public static T MapTo<T, TM>(this TM model) where T : class, new()
        where TM : class, new()
    {
        return _mapper!.Map<TM, T>(model);
    }

    public static T ToEntity<T>(this IDto dto) where T : class, new()
    {
        return _mapper!.Map<T>(dto);
    }

    public static T ToEntity<T>(this T dto) where T : class
    {
        return _mapper!.Map<T>(dto);
    }

    public static IEnumerable<T> ToEntity<T>(this IEnumerable<IDto> dto) where T : class, new()
    {
        return _mapper!.Map<IEnumerable<T>>(dto);
    }

    public static T ToDto<T>(this BaseEntity model) where T : class, new()
    {
        return _mapper!.Map<T>(model);
    }

    public static IEnumerable<T> ToDto<T>(this IEnumerable<BaseEntity> model) where T : class, new()
    {
        return _mapper!.Map<IEnumerable<T>>(model);
    }
}