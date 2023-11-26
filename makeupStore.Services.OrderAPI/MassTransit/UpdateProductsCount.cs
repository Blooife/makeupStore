using makeupStore.Services.OrderAPI.Models.Dto;

namespace makeupStore.Services.MassTransit;

public class UpdateProductsCount
{
    public IEnumerable<ProductDto> updatedProducts;
}