using makeupStore.Services.CartAPI.Services.IServices;
using MassTransit;

namespace makeupStore.Services.CartAPI.Consumers;

public class CleanCart: BaseConsumer, IConsumer<MassTransit.CleanCart>
{
    public CleanCart(ICartService cartService) : base(cartService)
    {
    }

    public async Task Consume(ConsumeContext<MassTransit.CleanCart> context)
    {
        await _cartService.CleanCart(context.Message.CartHeaderId);
    }
}