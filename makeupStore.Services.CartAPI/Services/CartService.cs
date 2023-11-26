using makeupStore.Services.CartAPI.Data;
using makeupStore.Services.CartAPI.Services.IServices;

namespace makeupStore.Services.CartAPI.Services;

public class CartService:ICartService
{
    private readonly AppDbContext _db;

    public CartService(AppDbContext db)
    {
        _db = db;
    }
    public async Task CleanCart(int cartHeaderId)
    {
        var cd = _db.CartDetails.Where(u => u.CartHeaderId == cartHeaderId).ToList();
        foreach (var item in cd)
        {
            _db.CartDetails.Remove(item);
        }

        var ch = _db.CartHeaders.FirstOrDefault(u => u.CartHeaderId == cartHeaderId);
        if(ch != null)
            _db.CartHeaders.Remove(ch);
        await _db.SaveChangesAsync();
    }
}