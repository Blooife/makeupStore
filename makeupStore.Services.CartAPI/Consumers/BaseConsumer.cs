using makeupStore.Services.CartAPI.Services.IServices;

namespace makeupStore.Services.CartAPI.Consumers;

public class BaseConsumer
{
    protected readonly ICartService _cartService;
    public BaseConsumer(ICartService cartService)
    {
        _cartService = cartService;
    }
}