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
            
        }
        else
        {
            list = new List<OrderHeaderDto>();
        }
        return Json(new { data = list.OrderByDescending(u=>u.OrderHeaderId) });
    }
    
    [HttpPost("OrderReadyForDelivery")]
    public async Task<IActionResult> OrderReadyForDelivery(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId,SD.Status_OnTheWay);
        var responseDto = await _orderService.GetOrderByIdAsync(orderId);
        OrderHeaderDto order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return View("OrderDetail", order);
        }
        return View("OrderDetail", order);
    }
    
    [HttpPost("OrderDelivered")]
    public async Task<IActionResult> OrderDelivered(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId,SD.Status_Delivered);
        var responseDto = await _orderService.GetOrderByIdAsync(orderId);
        OrderHeaderDto order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return View("OrderDetail", order);
        }
        return View("OrderDetail", order);
    }

    [HttpPost("CompleteOrder")]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Taken);
        var responseDto = await _orderService.GetOrderByIdAsync(orderId);
        OrderHeaderDto order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return View("OrderDetail", order);
        }
        return View("OrderDetail", order);
    }
    
    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
        var responseDto = await _orderService.GetOrderByIdAsync(orderId);
        OrderHeaderDto order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(responseDto.Result));
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }
        return View("OrderDetail", order);
    }
}    