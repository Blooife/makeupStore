

using makeupStore.Services.CartAPI.Models.Dto;

namespace makeupStore.Services.CartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
