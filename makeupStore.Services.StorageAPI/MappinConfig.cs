using AutoMapper;
using makeupStore.Services.StorageAPI.Models;
using makeupStore.Services.StorageAPI.Models.Dto;

namespace makeupStore.Services.StorageAPI;

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