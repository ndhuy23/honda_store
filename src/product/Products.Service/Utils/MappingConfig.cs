using AutoMapper;
using ProductService.Data.Dto;
using ProductService.Data.Entities;
using ProductService.Data.Template;


namespace ProductService.Utils
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<PostProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features))
                .ForMember(dest => dest.Preferentials, opt => opt.MapFrom(src => src.Preferentials))
                .ForMember(dest => dest.ExtendPreferentials, opt => opt.MapFrom(src => src.ExtendPreferentials))
                .ForMember(dest => dest.ColorIds, opt => opt.MapFrom(src => src.ColorQuantity.Keys.ToList()))
                .ReverseMap();
                
            });
            

            return mapperConfig;
        }
    }
}
