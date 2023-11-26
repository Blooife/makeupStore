using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;

namespace makeupStore.Web.Service;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;
    
    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    
    public async Task<ResponseDto?> GetOrdersByUserIdAsync(string userId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Data = userId,
            Url = SD.OrderAPIBase + "/api/OrderAPI/GetOrders"
        });
    }

    public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.OrderAPIBase + "/api/OrderAPI/CreateOrder"
        });
    }

    public async Task<ResponseDto?> GetOrderByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.OrderAPIBase + "/api/OrderAPI/GetOrder/" + id
        });
    }
    
    public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = newStatus,
            Url = SD.OrderAPIBase + "/api/OrderAPI/UpdateOrderStatus/" + orderId
        });
    }
}