using System.Collections;
using IdentityModel;
using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace makeupStore.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public ProductController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? list = await GetProducts();
        return View(list);
    }

    public async Task<List<ProductDto>?> GetProducts()
    {
        List<ProductDto>? list = new List<ProductDto>();
        ResponseDto? response = await _productService.GetAllProductsAsync();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
            var categories = list.Select(p => p.Category).Distinct().ToArray();
            var categoryList = new List<SelectListItem>();
            categoryList.Add(new SelectListItem{Text = "all", Value = "all"});
            foreach (var c in categories)
            {
                categoryList.Add(new SelectListItem{Text = c, Value = c});
            }
            Console.WriteLine(categoryList.First().Value);
            ViewBag.CategoryList = categoryList;
        }
        return list;
    }
    
    public async Task<IActionResult> ProductByCategory(string selectedCategory)
    {
        List<ProductDto>? list = await GetProducts();
        if (selectedCategory != "all")
        {
            list = list.Where(p => p.Category == selectedCategory).ToList();
        }
        return View("ProductIndex", list);
    }
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto? model = new();
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);
        if (response != null && response.IsSuccess)
        {
            model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
        }
        return View(model);
    }
    
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(ProductDto productDto)
    {
        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto
            {
                UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
            }
        };

        cartDto.CartDetails = new List<CartDetailsDto>(){new CartDetailsDto()
        {
            Count = productDto.Count,
            ProductId = productDto.ProductId,
           // Product = productDto,
        }};
        ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Item has been added to the Shopping Cart";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(productDto);
    }
}