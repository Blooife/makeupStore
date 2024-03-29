using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.ProductAPI.Data;
using makeupStore.Services.MassTransit.Responses;
using makeupStore.Services.ProductAPI.Models;
using makeupStore.Services.ProductAPI.Models.Dto;
using makeupStore.Services.ProductAPI.Services.IServices;

namespace makeupStore.Services.ProductAPI.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<GetAllProductsResponse> GetAllProducts()
    {
        return new GetAllProductsResponse(){Products = _db.Products.ToList()};
    }

    public async Task<GetProductByIdResponse> GetProductById(int id)
    {
        return new GetProductByIdResponse()
        {
           product =  _db.Products.First(u=>u.ProductId==id),
        };
    }

    public async Task<GetProductsForCartResponse> GetProductsForCart(IEnumerable<int> ids)
    {
        List<Product> pr = new List<Product>();
        foreach (var id in ids)
        {
            var product = _db.Products.FirstOrDefault(u => u.ProductId == id);
            if (product != null)
            {
                pr.Add(product);
            }
        }

        return new GetProductsForCartResponse()
        {
            Products = pr,
        };
    }

    public async Task UpdateProductsCount(IEnumerable<ProductDto> products)
    {
        Console.WriteLine("im in update pr count");
        foreach (var item in products)
        {
            var product = _db.Products.FirstOrDefault(u=>u.ProductId==item.ProductId);
            if (product != null)
            {
                product.Count -= item.Count;
                _db.Products.Update(product);
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task<BaseResponse> DeleteProduct(int id)
    {
        Product obj = _db.Products.First(u=>u.ProductId==id);
        _db.Products.Remove(obj);
        await _db.SaveChangesAsync();
        return new BaseResponse();
    }

    public async Task<BaseResponse> AddProduct(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return new BaseResponse();
    }

    public async Task<BaseResponse> UpdateProduct(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        return new BaseResponse();
    }
}