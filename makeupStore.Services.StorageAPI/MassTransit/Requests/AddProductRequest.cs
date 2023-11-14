using makeupStore.Services.StorageAPI.Models;

namespace makeupStore.Services.MassTransit.Requests;

public class AddProductRequest
{
    public Product product { get; set; }
}