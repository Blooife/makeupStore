using makeupStore.Services.ProductAPI.Models.Dto;

namespace makeupStore.Services.MassTransit;

public class UpdateProductsCount
{
    public IEnumerable<ProductDto> updatedProducts;
}