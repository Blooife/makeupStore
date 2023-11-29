using AutoMapper;
using makeupStore.Services.CartAPI.Controllers;
using makeupStore.Services.CartAPI.Data;
using makeupStore.Services.CartAPI.Models;
using makeupStore.Services.CartAPI.Models.Dto;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace CartServiceUnitTest;

public class UnitTest1
{
    private CartAPIController _cartApiController;
    private IBusControl _busControl;
    
    [Fact]
    public void TestGetCart()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            config.CreateMap<Product, ProductDto>().ReverseMap();
        });
        _busControl = Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.ReceiveEndpoint("queue_name", ep =>
            {
            });
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var cartContextMock = new Mock<AppDbContext>();
        cartContextMock.Setup<DbSet<CartHeader>>(x => x.CartHeaders).ReturnsDbSet(TestHelpers.GetFakeCartHeaders());
        cartContextMock.Setup<DbSet<CartDetails>>(x => x.CartDetails).ReturnsDbSet(TestHelpers.GetFakeCartDetailsList());
        
        _cartApiController = new CartAPIController(cartContextMock.Object, mapper, _busControl);
        var response = _cartApiController.GetCart("abc");
    }
    
    [Fact]
    public void TestIncDecCart()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            config.CreateMap<Product, ProductDto>().ReverseMap();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var cartContextMock = new Mock<AppDbContext>();
        cartContextMock.Setup<DbSet<CartHeader>>(x => x.CartHeaders).ReturnsDbSet(TestHelpers.GetFakeCartHeaders());
        cartContextMock.Setup<DbSet<CartDetails>>(x => x.CartDetails).ReturnsDbSet(TestHelpers.GetFakeCartDetailsList());
        
        _cartApiController = new CartAPIController(cartContextMock.Object, mapper, _busControl);
        var response = _cartApiController.IncCartCount(1);
        Assert.True(response.Result.IsSuccess);
        response = _cartApiController.DecCartCount(5);
        Assert.False(response.Result.IsSuccess);
    }
    
    [Fact]
    public void TestUpsertCart()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            config.CreateMap<Product, ProductDto>().ReverseMap();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var cartContextMock = new Mock<AppDbContext>();
        cartContextMock.Setup<DbSet<CartHeader>>(x => x.CartHeaders).ReturnsDbSet(TestHelpers.GetFakeCartHeaders());
        cartContextMock.Setup<DbSet<CartDetails>>(x => x.CartDetails).ReturnsDbSet(TestHelpers.GetFakeCartDetailsList());
        CartDto cdto = new CartDto()
        {
            CartHeader = new CartHeaderDto()
            {
                CartHeaderId = 3,
                CartTotal = 0,
                UserId = "user"
            },
            CartDetails = new List<CartDetailsDto>()
            {
                new CartDetailsDto()
                {
                    CartDetailsId = 12,
                    CartHeaderId = 3,
                    Count = 10,
                    ProductId = 4,
                }
            }
        };
        _cartApiController = new CartAPIController(cartContextMock.Object, mapper, _busControl);
        var response = _cartApiController.CartUpsert(cdto);
        Assert.True(response.Result.IsSuccess);
    }
    
    [Fact]
    public void TestRemove()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            config.CreateMap<Product, ProductDto>().ReverseMap();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var cartContextMock = new Mock<AppDbContext>();
        cartContextMock.Setup<DbSet<CartHeader>>(x => x.CartHeaders).ReturnsDbSet(TestHelpers.GetFakeCartHeaders());
        cartContextMock.Setup<DbSet<CartDetails>>(x => x.CartDetails).ReturnsDbSet(TestHelpers.GetFakeCartDetailsList());
        
        _cartApiController = new CartAPIController(cartContextMock.Object, mapper, m.Object);
        var response = _cartApiController.RemoveCart(1);
        Assert.True(response.Result.IsSuccess);
    }
}