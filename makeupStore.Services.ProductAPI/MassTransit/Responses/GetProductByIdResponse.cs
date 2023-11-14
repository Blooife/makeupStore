using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.MassTransit.Responses;

public class GetProductByIdResponse
{
    public Product product { get; set; }
}