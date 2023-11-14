
using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.MassTransit.Responses;
using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.ProductAPI.Services.IServices;

public interface IProductService
{
    public Task<GetAllProductsResponse> GetAllProducts();
    public Task<GetProductByIdResponse> GetProductById(int id);
    public Task<BaseResponse> DeleteProduct(int id);
    public Task<BaseResponse> AddProduct(Product product);
    public Task<BaseResponse> UpdateProduct(Product product);

    public Task<GetProductsForCartResponse> GetProductsForCart(IEnumerable<int> ids);
    //public async Task<Product> GetAllProducts()
}