using AutoMapper;
using makeupStore.Services.ProductAPI.Data;
using makeupStore.Services.ProductAPI.Models;
using makeupStore.Services.ProductAPI.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace makeupStore.Services.ProductAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductAPIController : ControllerBase
{
    private readonly IBusControl _bus;
    private readonly AppDbContext db;
    private ResponseDto response;
    private IMapper mapper;

    public ProductAPIController(AppDbContext db, IMapper mapper, IBusControl bus)
    {
        this.db = db;
        response = new ResponseDto();
        this.mapper = mapper;
        _bus = bus;
    }

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            IEnumerable<Product> objList = db.Products.ToList();
            response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public ResponseDto Get(int id)
    {
        try
        {
            Product objList = db.Products.First(u=>u.ProductId==id);
            response.Result = mapper.Map<ProductDto>(objList);
            
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
    }
    
    [HttpGet]
    [Route("GetByCategory/{category}")]
    public ResponseDto GetByCategory(string category)
    {
        try
        {
            IEnumerable<Product> objList = db.Products.Where(u=>u.Category==category).ToList();
            response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);
            
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
    }
    
    [HttpGet]
    [Route("GetSortedMax")]
    public ResponseDto GetSortedMax()
    {
        try
        {
            IEnumerable<Product> objList = db.Products.OrderByDescending(u=>u.Price).ToList();
            response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
    }
    
    [HttpGet]
    [Route("GetSortedMin")]
    public ResponseDto GetSortedMin()
    {
        try
        {
            IEnumerable<Product> objList = db.Products.OrderBy(u=>u.Price).ToList();
            response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
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