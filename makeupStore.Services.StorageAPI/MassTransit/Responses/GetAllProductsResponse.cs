using makeupStore.Services.StorageAPI.Models;

namespace makeupStore.Services.MassTransit.Responses
{
    public class GetAllProductsResponse
    {
        public List<Product> Products { get; set; }
    }
}

