using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace makeupStore.Web.Controllers;

public class StorageController : Controller
{
    private readonly IStorageService _storageService;

    public StorageController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<IActionResult> StorageIndex()
    {
        List<ProductDto>? list = new List<ProductDto>();
        ResponseDto? response = await _storageService.GetAllProductsAsync();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
        }
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> StorageCreate(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await _storageService.CreateProductAsync(model);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(StorageIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(model);
    }
    
    public async Task<IActionResult> StorageGetById(int ProductId)
    {
        ResponseDto? response = await _storageService.GetProductByIdAsync(ProductId);

        if (response != null && response.IsSuccess)
        {
            ProductDto? model= JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }
   
    [HttpPost]
    public async Task<IActionResult> StorageDelete(ProductDto ProductDto)
    {
        ResponseDto? response = await _storageService.DeleteProductAsync(ProductDto.ProductId);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction(nameof(StorageIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(ProductDto);
    }
    
    public async Task<IActionResult> StorageDetails(int productId)
    {
        ProductDto? model = new();
        ResponseDto? response = await _storageService.GetProductByIdAsync(productId);
        if (response != null && response.IsSuccess)
        {
            model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
        }
        else
        {
            return NotFound();
        }
        return View(model);
    }
}