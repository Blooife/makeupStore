using AutoMapper;
using GreenPipes;
using MassTransit;
using Newtonsoft.Json;
using makeupStore.Services.StorageAPI;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

IMapper mapper = MappinConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        
        cfg.Host(new Uri("rabbitmq://localhost"));
        cfg.ReceiveEndpoint("storage-queue", e =>
        {
            e.PrefetchCount = 20;
            e.UseMessageRetry(r => r.Interval(2, 100));
          
        });
        cfg.ConfigureEndpoints(context);

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

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();