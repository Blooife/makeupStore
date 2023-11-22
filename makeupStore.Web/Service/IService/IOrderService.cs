using makeupStore.Web.Models;

namespace makeupStore.Web.Service.IService;

public interface IOrderService
{
    public Task<ResponseDto?> GetOrdersByUserIdAsync(string userId);
    public Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
    public Task<ResponseDto?> GetOrderByIdAsync(int id);
}