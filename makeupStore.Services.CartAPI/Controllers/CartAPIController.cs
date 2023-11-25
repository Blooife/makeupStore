using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using makeupStore.Services.CartAPI.Data;
using makeupStore.Services.CartAPI.Models;
using makeupStore.Services.CartAPI.Models.Dto;
using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.MassTransit.Responses;
using MassTransit;

namespace makeupStore.Services.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IBusControl _bus;
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        public CartAPIController(AppDbContext db, IMapper mapper, IBusControl bus)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
            _bus = bus;
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails.Where(
                    u => u.CartHeaderId == cart.CartHeader.CartHeaderId));
                List<int> prIds = new List<int>();
                foreach (var item in cart.CartDetails)
                {
                    prIds.Add(item.ProductId);
                }
                var products = await GetResponseRabbitTask<GetProductsForCartRequest, GetProductsForCartResponse>(
                    new GetProductsForCartRequest()
                    {
                        ProductsIds = prIds,
                    });
                cart.CartHeader.CartTotal = 0;
                foreach (var p in products.Products)
                {
                    var c =cart.CartDetails.First(u => u.ProductId == p.ProductId);
                    c.Product = _mapper.Map<ProductDto>(p);
                    cart.CartHeader.CartTotal += c.Product.Price * c.Count;
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        
        [HttpPost("DecCartCount")]
        public async Task<ResponseDto> DecCartCount([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetails =  _db.CartDetails.FirstOrDefault(u=>u.CartDetailsId==cartDetailsId);
                if (cartDetails != null)
                {
                    cartDetails.Count--;
                    _db.Update(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find cartDetails by Id";
                }
            }
            catch (Exception ex)
            {
                _response.Message= ex.Message.ToString();
                _response.IsSuccess= false;
            }
            return _response;
        }
        
        [HttpPost("IncCartCount")]
        public async Task<ResponseDto> IncCartCount([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetails =  _db.CartDetails.FirstOrDefault(u=>u.CartDetailsId==cartDetailsId);
                if (cartDetails != null)
                {
                    cartDetails.Count++;
                    _db.Update(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find cartDetails by Id";
                }
            }
            catch (Exception ex)
            {
                _response.Message= ex.Message.ToString();
                _response.IsSuccess= false;
            }
            return _response;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message= ex.Message.ToString();
                _response.IsSuccess= false;
            }
            return _response;
        }
        
        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                Console.WriteLine("remove");
                CartDetails cartDetails = _db.CartDetails
                   .First(u => u.CartDetailsId == cartDetailsId);

                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
               
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        
        private async Task<TOut> GetResponseRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            //to do: take uri form confic(depndncy injctn)
            var client = _bus.CreateRequestClient<TIn>(new Uri("rabbitmq://localhost/product-queue"));
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
