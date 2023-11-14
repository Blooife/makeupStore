using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class AddProduct : BaseConsumer, IConsumer<AddProductRequest>
{
    public AddProduct(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<AddProductRequest> context)
    {
        var order = await _productService.AddProduct(context.Message.product);
        await context.RespondAsync(order);
    }
}