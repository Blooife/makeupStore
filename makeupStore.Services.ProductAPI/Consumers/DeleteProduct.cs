using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class DeleteProduct : BaseConsumer, IConsumer<DeleteProductRequest>
{
    public DeleteProduct(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<DeleteProductRequest> context)
    {
        var order = await _productService.DeleteProduct(context.Message.id);
        await context.RespondAsync(order);
    }
}