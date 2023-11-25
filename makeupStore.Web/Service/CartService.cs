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
            Url = SD.CartAPIBase + "/api/CartAPI/GetCart/" + userId
        });
    }
    
    public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
    { 
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDetailsId,
            Url = SD.CartAPIBase + "/api/CartAPI/RemoveCart"
        });
    }

    public async Task<ResponseDto?> IncCartCountAsync(int cartDetailsId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDetailsId,
            Url = SD.CartAPIBase + "/api/CartAPI/IncCartCount"
        });
    }

    public async Task<ResponseDto?> DecCartCountAsync(int cartDetailsId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDetailsId,
            Url = SD.CartAPIBase + "/api/CartAPI/DecCartCount"
        });
    }


    public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.CartAPIBase + "/api/CartAPI/CartUpsert"
        });
    }
}