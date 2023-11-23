using makeupStore.Services.ProductAPI.Models;

namespace makeupStore.Services.MassTransit.Requests;

public class UpdateProductRequest
{
    public Product product { get; set; }
}