using makeupStore.Services.CartAPI.Models.Dto;

namespace makeupStore.Services.MassTransit;


public class UpdateProductsCount
{
    public IEnumerable<ProductDto> updatedProducts;
}