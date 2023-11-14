using AutoMapper;
using makeupStore.Services.ProductAPI.Models;
using makeupStore.Services.ProductAPI.Models.Dto;

namespace makeupStore.Services.ProductAPI;

public class MappinConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDto, Product>();
            config.CreateMap<Product, ProductDto>();
        });
        return mappingConfig;
    }
}