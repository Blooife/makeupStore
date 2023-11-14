

using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.MassTransit.Responses;

public class GetProductsForCartResponse
{
    public IEnumerable<Product> Products { get; set; }
}