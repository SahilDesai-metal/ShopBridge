using AutoMapper;
using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductDTOs;

namespace ShopBridge.ServiceLayer.MapperProfile
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductCreationDto>()
                .ForMember(dest => dest.ProductDescription, src => src.MapFrom(source => source.Description))
                .ForMember(dest => dest.MRP, src => src.MapFrom(source => source.Price))
                .ForMember(dest => dest.AvailableQuantity, src => src.MapFrom(source => source.Quantity));

            CreateMap<ProductCreationDto, Product>()
                .ForMember(dest => dest.Description, src => src.MapFrom(source => source.ProductDescription))
                .ForMember(dest => dest.Price, src => src.MapFrom(source => source.MRP))
                .ForMember(dest => dest.Quantity, src => src.MapFrom(source => source.AvailableQuantity));

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Description, src => src.MapFrom(source => source.ProductDescription))
                .ForMember(dest => dest.Price, src => src.MapFrom(source => source.MRP))
                .ForMember(dest => dest.Quantity, src => src.MapFrom(source => source.AvailableQuantity));
        }
    }
}
