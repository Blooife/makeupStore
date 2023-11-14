using makeupStore.Services.ProductAPI.Services.IServices;

namespace makeupStore.Services.ProductAPI.Consumers;

public class BaseConsumer
{
    protected readonly IProductService _productService;
    public BaseConsumer(IProductService prService)
    {
        _productService = prService;
    }
}