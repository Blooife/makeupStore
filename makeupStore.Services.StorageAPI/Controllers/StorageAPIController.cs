using AutoMapper;
using makeupStore.Services.MassTransit.Requests;
using makeupStore.Services.MassTransit.Responses;
using makeupStore.Services.StorageAPI.Models;
using makeupStore.Services.StorageAPI.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace makeupStore.Services.StorageAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]

public class StorageAPIController : ControllerBase
{
    private readonly IBusControl _busControl;
    private ResponseDto responseDto;
    private IMapper _mapper;
    private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/product-queue");

    public StorageAPIController(IBusControl bus, IMapper map)
    {
        _busControl = bus;
        _mapper = map;
        responseDto = new ResponseDto();
    }
    
    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {   
            var response = await GetResponseRabbitTask<GetAllProductsRequest, GetAllProductsResponse>(new GetAllProductsRequest());
            IEnumerable<Product> objList = response.Products;
            responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            return Ok(responseDto);
        }
        catch (Exception e)
        {
            responseDto.IsSuccess = false;
            responseDto.Message = e.Message;
            return Problem(JsonConvert.SerializeObject(responseDto));
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var response = await GetResponseRabbitTask<GetProductByIdRequest, GetProductByIdResponse>(new GetProductByIdRequest()
            {
                id = id,
            });
            responseDto.Result = _mapper.Map<ProductDto>(response.product);
            return Ok(responseDto);
        }
        catch (Exception e)
        {
            responseDto.IsSuccess = false;
            responseDto.Message = e.Message;
            return Problem(JsonConvert.SerializeObject(responseDto));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
    {
        try
        {
            var response = await GetResponseRabbitTask<AddProductRequest, BaseResponse>(new AddProductRequest()
            {
                product = _mapper.Map<ProductDto,Product>(productDto),
            });
            responseDto.Result = response;
            return Ok(responseDto);
        }
        catch (Exception e)
        {
            responseDto.IsSuccess = false;
            responseDto.Message = e.Message;
            return Problem(JsonConvert.SerializeObject(responseDto));
        }
    }
    
    [HttpPut]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
    {
        try
        {
            var response = await GetResponseRabbitTask<UpdateProductRequest, BaseResponse>(new UpdateProductRequest()
            {
                product = _mapper.Map<ProductDto,Product>(productDto),
            });
            responseDto.Result = response;
            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            responseDto.IsSuccess = false;
            responseDto.Message = ex.Message;
            return Problem(JsonConvert.SerializeObject(responseDto));
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var response = await GetResponseRabbitTask<DeleteProductRequest, BaseResponse>(new DeleteProductRequest()
            {
                id = id,
            });
            responseDto.Result = response;
            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            responseDto.IsSuccess = false;
            responseDto.Message = ex.Message;
            return Problem(JsonConvert.SerializeObject(responseDto));
        }
    }
    
    private async Task<TOut> GetResponseRabbitTask<TIn, TOut>(TIn request)
        where TIn : class
        where TOut : class
    {
        var client = _busControl.CreateRequestClient<TIn>(_rabbitMqUrl);
        var response = await client.GetResponse<TOut>(request);
        return response.Message;
    }
}