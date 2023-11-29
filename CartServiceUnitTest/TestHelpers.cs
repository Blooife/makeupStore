using makeupStore.Services.CartAPI.Models;

namespace CartServiceUnitTest;

public class TestHelpers
{
    public static List<CartHeader> GetFakeCartHeaders()
    {
        return new List<CartHeader>()
        {
            new CartHeader()
            {
                CartHeaderId = 0,
                CartTotal = 0,
                UserId = "abc",
            },
            new CartHeader()
            {
            CartHeaderId = 1,
            CartTotal = 0,
            UserId = "someUser",
        }
        };
    }
    
    public static List<CartDetails> GetFakeCartDetailsList()
    {
        return new List<CartDetails>()
        {
            new CartDetails()
            {
                CartDetailsId = 0,
                CartHeaderId = 0,
                Count = 3,
                ProductId = 0,
            },
            new CartDetails()
            {
                CartDetailsId = 1,
                CartHeaderId = 0,
                Count = 5,
                ProductId = 1,
            },
            new CartDetails()
            {
                CartDetailsId = 2,
                CartHeaderId = 1,
                Count = 2,
                ProductId = 0,
            }
        };
    }
}