
using makeupStore.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Utility;

namespace OrderServiceUnitTest;

public class TestHelpers
{
    public static List<OrderHeader> GetFakeOrderHeaders()
    {
        return new List<OrderHeader>()
        {
            new OrderHeader()
            {
                OrderHeaderId = 0,
                Address = "some address",
                Email = "some email",
                Name = "name",
                OrderDetails = new List<OrderDetails>()
                {
                    
                },
                OrderTime = new DateTime(2023,10,13),
                OrderTotal = 0,
                Phone = "1234",
                Status = SD.Status_Pending,
                UserId = "abc"
            }
        };
    }

    public static List<OrderDetails> GetFakeOrderDetails()
    {
        return new List<OrderDetails>()
        {
            new OrderDetails()
            {
                OrderDetailsId = 0,
                OrderHeaderId = 0,
                Price = 15,
                ProductId = 13,
                ProductName = "somename"
            },
            new OrderDetails()
            {
                OrderDetailsId = 1,
                OrderHeaderId = 0,
                Price = 13,
                ProductId = 10,
                ProductName = "name"
            }
        };
    }
}