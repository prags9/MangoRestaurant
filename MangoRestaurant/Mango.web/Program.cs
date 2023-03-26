using Mango.web;
using Mango.web.Services;
using Mango.web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
SD.CouponAPIBase  = builder.Configuration["ServiceUrls:CouponAPI"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";

}).AddCookie("Cookies", opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
}).AddOpenIdConnect("oidc", c =>
{
    c.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
    c.GetClaimsFromUserInfoEndpoint = true;
    c.ClientId = "mango";
    c.ClientSecret = "secret key Eg";
    c.ResponseType = "code";
    c.ClaimActions.MapJsonKey("role", "role", "rol");
    c.ClaimActions.MapJsonKey("sub", "sub", "sub");

    c.TokenValidationParameters.NameClaimType = "name";
    c.TokenValidationParameters.RoleClaimType = "role";
    c.Scope.Add("mango");
    c.SaveTokens = true;
});

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
