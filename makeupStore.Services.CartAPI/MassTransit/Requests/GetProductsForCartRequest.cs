namespace makeupStore.Services.MassTransit.Requests;

public class GetProductsForCartRequest
{
    public IEnumerable<int> ProductsIds { get; set; }
}