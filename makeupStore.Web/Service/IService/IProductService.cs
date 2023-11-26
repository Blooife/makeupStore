using makeupStore.Web.Models;

namespace makeupStore.Web.Service.IService;

public interface IProductService
{
    Task<ResponseDto?> GetAllProductsAsync();
    Task<ResponseDto?> GetProductByIdAsync(int id);
    
}