using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class GetProductById : BaseConsumer, IConsumer<GetProductByIdRequest>
{
    public GetProductById(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<GetProductByIdRequest> context)
    {
        var order = await _productService.GetProductById(context.Message.id);
        await context.RespondAsync(order);
    }
}