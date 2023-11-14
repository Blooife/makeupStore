using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class GetProductsForCart : BaseConsumer, IConsumer<GetProductsForCartRequest>
{
    public GetProductsForCart(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<GetProductsForCartRequest> context)
    {
        var order = await _productService.GetProductsForCart(context.Message.ProductsIds);
                await context.RespondAsync(order);
    }
}