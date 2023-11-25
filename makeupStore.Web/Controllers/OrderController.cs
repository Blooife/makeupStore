using IdentityModel;
using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace makeupStore.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [Authorize]
    public async Task<IActionResult> OrderIndex()
    {
        IEnumerable<OrderHeaderDto> list = new List<OrderHeaderDto>();
        string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        var response = await _orderService.GetOrdersByUserIdAsync(userId);
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
        }

        return View(list);
    }
    
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
        string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

        var response = await _orderService.GetOrderByIdAsync(orderId);
        if (response != null && response.IsSuccess)
        {
            orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
        }
        if(!User.IsInRole(SD.RoleAdmin) && userId!= orderHeaderDto.UserId)
        {
            return NotFound();
        }
        return View(orderHeaderDto);
    }

    
    [HttpGet]
    public IActionResult GetAllOrders(string status) 
    {
        IEnumerable<OrderHeaderDto> list;
        string userId = "";
        if (!User.IsInRole(SD.RoleAdmin))
        {
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        }
        ResponseDto response = _orderService.GetOrdersByUserIdAsync(userId).GetAwaiter().GetResult();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
            switch (status)
            {
                case "approved":
                    list = list.Where(u => u.Status == SD.Status_Approved);
                    break;
                case "readyforpickup":
                    list = list.Where(u => u.Status == SD.Status_ReadyForPickup);
                    break;
                case "cancelled":
                    list = list.Where(u => u.Status == SD.Status_Cancelled || u.Status == SD.Status_Refunded);
                    break;
                default:
                    break;
            }
        }
        else
        {
            list = new List<OrderHeaderDto>();
        }
        return Json(new { data = list.OrderByDescending(u=>u.OrderHeaderId) });
    }
}    