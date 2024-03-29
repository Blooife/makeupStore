using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class UpdateProduct : BaseConsumer, IConsumer<UpdateProductRequest>
{
    public UpdateProduct(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<UpdateProductRequest> context)
    {
        var order = await _productService.UpdateProduct(context.Message.product);
        await context.RespondAsync(order);
    }
}