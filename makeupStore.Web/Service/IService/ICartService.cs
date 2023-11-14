using makeupStore.Web.Models;

namespace makeupStore.Web.Service.IService;

public interface ICartService
{
    Task<ResponseDto?> GetCartByUserIdAsync(string userId);
}