using AutoMapper;
using GreenPipes;
using makeupStore.Services.ProductAPI;
using makeupStore.Services.ProductAPI.Consumers;
using makeupStore.Services.ProductAPI.Data;
using makeupStore.Services.ProductAPI.Extentions;
using makeupStore.Services.ProductAPI.Services;
using makeupStore.Services.ProductAPI.Services.IServices;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

IMapper mapper = MappinConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductService, ProductService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GetAllProducts>();
    x.AddConsumer<GetProductById>();
    x.AddConsumer<DeleteProduct>();
    x.AddConsumer<AddProduct>();
    x.AddConsumer<UpdateProduct>();
    x.AddConsumer<GetProductsForCart>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"));
        cfg.ReceiveEndpoint("product-queue", e =>
        {
            e.PrefetchCount = 20;
            e.UseMessageRetry(r => r.Interval(2, 100));
            e.Consumer<GetAllProducts>(context);
            e.Consumer<GetProductById>(context);
            e.Consumer<DeleteProduct>(context);
            e.Consumer<AddProduct>(context);
            e.Consumer<UpdateProduct>(context);
            e.Consumer<GetProductsForCart>(context);
        });
        cfg.ConfigureJsonSerializer(settings =>
        {
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            return settings;
        });
        cfg.ConfigureJsonDeserializer(configure =>
        {
            configure.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            return configure;
        });
    });

});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}