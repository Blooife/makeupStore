namespace makeupStore.Services.CartAPI.Services.IServices;

public interface ICartService
{
    public Task CleanCart(int cartHeaderId);
}