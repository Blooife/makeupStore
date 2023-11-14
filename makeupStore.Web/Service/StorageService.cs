using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;

namespace makeupStore.Web.Service;

public class StorageService : IStorageService
{
    private readonly IBaseService _baseService;
    
    public StorageService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> GetAllProductsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/productAPI"
        });
    }

    public async Task<ResponseDto?> GetProductByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/storageAPI/" + id
        });
    }

    public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data=productDto,
            Url = SD.ProductAPIBase + "/api/storageAPI" 
        });
    }

    public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Data=productDto,
            Url = SD.ProductAPIBase + "/api/storageAPI" 
        });
    }

    public async Task<ResponseDto?> DeleteProductAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/storageAPI/" + id
        });
    }
}