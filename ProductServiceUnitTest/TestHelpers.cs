using makeupStore.Services.ProductAPI.Models;

namespace ProductServiceUnitTest;

public class TestHelpers
{
    public static List<Product> GetFakeProductList()
    {
        return new List<Product>()
        {
            new Product()
            {
                ProductId = 0,
                Name = "John Doe",
                Category = "",
                Count = 5,
                Description = "",
                ImageUrl = "df",
                Price = 10.2,
            },
            new Product()
            {
                ProductId = 1,
                Name = "John",
                Category = "",
                Count = 5,
                Description = "",
                ImageUrl = "df",
                Price = 10.2,
            },
        };
    }
}