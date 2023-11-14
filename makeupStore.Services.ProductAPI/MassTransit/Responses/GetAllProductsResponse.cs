using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.MassTransit.Responses
{
    public class GetAllProductsResponse
    {
        public List<Product> Products { get; set; }
    }
}

