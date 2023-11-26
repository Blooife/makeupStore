
using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.MassTransit.Responses;
using makeupStore.Services.ProductAPI.Models;
using makeupStore.Services.ProductAPI.Models.Dto;

namespace makeupStore.Services.ProductAPI.Services.IServices;

public interface IProductService
{
    public Task<GetAllProductsResponse> GetAllProducts();
    public Task<GetProductByIdResponse> GetProductById(int id);
    public Task<BaseResponse> DeleteProduct(int id);
    public Task<BaseResponse> AddProduct(Product product);
    public Task<BaseResponse> UpdateProduct(Product product);

    public Task<GetProductsForCartResponse> GetProductsForCart(IEnumerable<int> ids);
    public Task UpdateProductsCount(IEnumerable<ProductDto> products);
    //public async Task<Product> GetAllProducts()
}