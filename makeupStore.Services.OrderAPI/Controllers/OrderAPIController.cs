using AutoMapper;
using makeupStore.Services.MassTransit;
using makeupStore.Services.OrderAPI.Data;
using makeupStore.Services.OrderAPI.Models;
using makeupStore.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Utility;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace makeupStore.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IPublishEndpoint _publishEndpoint;
        public OrderAPIController(AppDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        
        [HttpGet("GetOrders")]
        [Authorize]
        public ResponseDto? Get([FromBody] string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    objList = _db.OrderHeaders.Include(u => u.OrderDetails).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _db.OrderHeaders.Include(u => u.OrderDetails).Where(u=>u.UserId==userId).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
            }
            catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        
        [HttpGet("GetOrder/{id:int}")]
        [Authorize]
        public ResponseDto? Get(int id)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.Include(u => u.OrderDetails).First(u => u.OrderHeaderId == id);
                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
                orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);
                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
                await _publishEndpoint.Publish(new CleanCart()
                {
                    CartHeaderId = cartDto.CartHeader.CartHeaderId,
                });
                var updProducts = new List<ProductDto>();
                foreach (var item in cartDto.CartDetails)
                {
                    updProducts.Add(new ProductDto()
                    {
                        ProductId = item.ProductId,
                        Count = item.Count,
                    });
                }
                if (!updProducts.Equals(null))
                {
                    await _publishEndpoint.Publish(new UpdateProductsCount()
                    {
                        updatedProducts = updProducts,
                    });
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message=ex.Message;
            }
            return _response;
        }

        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    orderHeader.Status = newStatus;
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
