using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;

namespace makeupStore.Web.Service;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;
    
    public ProductService(IBaseService baseService)
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
            Url = SD.ProductAPIBase + "/api/productAPI/" + id
        });
    }

    public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data=productDto,
            Url = SD.ProductAPIBase + "/api/productAPI" 
        });
    }

    public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto?> DeleteProductsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/productAPI/" + id
        });
    }
}