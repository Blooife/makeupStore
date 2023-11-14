using makeupStore.Web.Models;

namespace makeupStore.Web.Service.IService;

public interface IStorageService
{
    Task<ResponseDto?> GetAllProductsAsync();
    Task<ResponseDto?> GetProductByIdAsync(int id);
    Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
    Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
    Task<ResponseDto?> DeleteProductAsync(int id);
}