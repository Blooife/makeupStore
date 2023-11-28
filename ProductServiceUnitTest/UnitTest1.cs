using AutoMapper;
using makeupStore.Services.ProductAPI.Controllers;
using makeupStore.Services.ProductAPI.Data;
using makeupStore.Services.ProductAPI.Models;
using makeupStore.Services.ProductAPI.Models.Dto;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;

namespace ProductServiceUnitTest;

public class UnitTest1
{
    private ProductAPIController _productApiController;
    
    [Fact]
    public void Test1()
    {
        var mockM = new Mock<IMapper>();
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDto, Product>();
            config.CreateMap<Product, ProductDto>();
        });
        var mapper = mappingConfig.CreateMapper();
        var m = new Mock<IBusControl>();
        
        
        var employeeContextMock = new Mock<AppDbContext>();
        employeeContextMock.Setup<DbSet<Product>>(x => x.Products).ReturnsDbSet(TestHelpers.GetFakeProductList());
        _productApiController = new ProductAPIController(employeeContextMock.Object, mapper, m.Object);
        var r = _productApiController.Get();

    }
}