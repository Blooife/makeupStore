using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.ProductAPI.Consumers;

public class UpdateProductsCount:BaseConsumer, IConsumer<MassTransit.UpdateProductsCount>
{
    public UpdateProductsCount(IProductService prService) : base(prService)
    {
    }

    public async Task Consume(ConsumeContext<MassTransit.UpdateProductsCount> context)
    {
        await _productService.UpdateProductsCount(context.Message.updatedProducts);
    }
}