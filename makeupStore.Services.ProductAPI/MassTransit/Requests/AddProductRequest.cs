using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.MassTransit.Requests;

public class AddProductRequest
{
    public Product product { get; set; }
}