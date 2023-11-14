using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class GetAllProducts : BaseConsumer, IConsumer<GetAllProductsRequest>
{
    public GetAllProducts(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<GetAllProductsRequest> context)
    {
        var order = await _productService.GetAllProducts();
        await context.RespondAsync(order);
    }
}