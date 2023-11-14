using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;

namespace makeupStore.Web.Service;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;
    
    public CartService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CartAPIBase + "/api/cartAPI/GetCart/" + userId
        });
    }
}