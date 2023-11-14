using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace makeupStore.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? list = new List<ProductDto>();
        ResponseDto? response = await _productService.GetAllProductsAsync();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
        }
        return View(list);
    }
}