using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace makeupStore.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    

    public CartController(ICartService cartService,IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }
    
    private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
    {
        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
        if(response!=null & response.IsSuccess)
        {
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            return cartDto;
        }
        return new CartDto();
    }
    
    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }
    
    [HttpPost]
    [ActionName("Checkout")]
    public async Task<IActionResult> Checkout(CartDto cartDto)
    {
        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        foreach (var item in cart.CartDetails)
        {
            if (item.Product.Count < item.Count)
            {
                ///////сообщение что на складе не хватает товара
                return RedirectToAction(nameof(CartIndex));
            }
        }
        cart.CartHeader.Phone = cartDto.CartHeader.Phone;
        cart.CartHeader.Email = cartDto.CartHeader.Email;
        cart.CartHeader.Name = cartDto.CartHeader.Name;
        var response = await _orderService.CreateOrderAsync(cart);
        OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

        return RedirectToAction(nameof(CartIndex));
    }
    
    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
        if (response != null & response.IsSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return RedirectToAction(nameof(CartIndex));
    }
    public async Task<IActionResult> CountDec(int CartDetailsId)
    {
        Console.WriteLine(CartDetailsId);
        ResponseDto? response = await _cartService.DecCartCountAsync(CartDetailsId);
        return RedirectToAction(nameof(CartIndex));
    }
    public async Task<IActionResult> CountInc(int CartDetailsId)
    {
        Console.WriteLine(CartDetailsId);
        ResponseDto? response = await _cartService.IncCartCountAsync(CartDetailsId);
        return RedirectToAction(nameof(CartIndex));
    }
}    