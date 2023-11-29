using System.Security.Claims;
using AutoMapper;
using makeupStore.Services.OrderAPI.Controllers;
using makeupStore.Services.OrderAPI.Data;
using makeupStore.Services.OrderAPI.Models;
using makeupStore.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Utility;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace OrderServiceUnitTest;

public class UnitTest1
{
    private OrderAPIController _orderApiController;
    
    [Fact]
    public void TestGetOrders()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest=>dest.CartTotal, u=>u.MapFrom(src=>src.OrderTotal)).ReverseMap();

            config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

            config.CreateMap<OrderDetailsDto, CartDetailsDto>();

            config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();

        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var orderContextMock = new Mock<AppDbContext>();
        orderContextMock.Setup<DbSet<OrderHeader>>(x => x.OrderHeaders).ReturnsDbSet(TestHelpers.GetFakeOrderHeaders());
        _orderApiController = new OrderAPIController(orderContextMock.Object, mapper, m.Object);
    }
    
    [Fact]
    public void TestGetOrder()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest=>dest.CartTotal, u=>u.MapFrom(src=>src.OrderTotal)).ReverseMap();

            config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

            config.CreateMap<OrderDetailsDto, CartDetailsDto>();
            config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var orderContextMock = new Mock<AppDbContext>();
        orderContextMock.Setup<DbSet<OrderHeader>>(x => x.OrderHeaders).ReturnsDbSet(TestHelpers.GetFakeOrderHeaders());
        _orderApiController = new OrderAPIController(orderContextMock.Object, mapper, m.Object);
        var response = _orderApiController.Get(0);
        var resp = mapper.Map<OrderHeaderDto>(response.Result);
        Assert.True(response.IsSuccess);
        Assert.Equal("name", resp.Name);
    }
    
    [Fact]
    public void TestUpdateStatus()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest=>dest.CartTotal, u=>u.MapFrom(src=>src.OrderTotal)).ReverseMap();

            config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

            config.CreateMap<OrderDetailsDto, CartDetailsDto>();
            config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        var orderContextMock = new Mock<AppDbContext>();
        orderContextMock.Setup<DbSet<OrderHeader>>(x => x.OrderHeaders).ReturnsDbSet(TestHelpers.GetFakeOrderHeaders());
        _orderApiController = new OrderAPIController(orderContextMock.Object, mapper, m.Object);
        var response = _orderApiController.UpdateOrderStatus(0,SD.Status_Delivered);
        Assert.True(response.Result.IsSuccess);
    }
}