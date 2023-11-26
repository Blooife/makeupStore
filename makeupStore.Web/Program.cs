using makeupStore.Web.Service;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IStorageService, StorageService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.StorageAPIBase = builder.Configuration["ServiceUrls:StorageAPI"];
SD.CartAPIBase = builder.Configuration["ServiceUrls:CartAPI"];
SD.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(optioins =>
{
    optioins.ExpireTimeSpan = TimeSpan.FromHours(10);
    optioins.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Auth/Login");;
    optioins.AccessDeniedPath = "/Auth/AccessDenied";
    /*optioins.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = (context) =>
        {
            context.HttpContext.Response.Redirect(SD.AuthAPIBase+"/api/AuthAPI/login");
            return Task.CompletedTask;
        }
    };*/
});

//builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();