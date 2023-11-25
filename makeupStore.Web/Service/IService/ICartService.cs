using makeupStore.Web.Models;

namespace makeupStore.Web.Service.IService;

public interface ICartService
{
    public Task<ResponseDto?> GetCartByUserIdAsync(string userId);
    public Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
    public Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
    public Task<ResponseDto?> IncCartCountAsync(int cartDetailsId);
    public Task<ResponseDto?> DecCartCountAsync(int cartDetailsId);
}