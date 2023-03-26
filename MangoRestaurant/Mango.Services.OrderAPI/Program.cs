using AutoMapper;
using Mango.Messages;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Mango.Services.OrderAPI.Repository;
using Mango.Services.OrderAPI.Messaging;
using Mango.Services.OrderAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();//op => op.Filters.Add()
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
//IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

var opBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
opBuilder.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
builder.Services.AddSingleton(new OrderRepository(opBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", opt =>
{
    opt.Authority = "https://localhost:7228";
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mango");
    });

});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.ShoppingCartAPI", Version = "v1" });
    //c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"ENter 'Bearer' [space] and your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
    }
    });


});

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
app.UseAzureServiceBusConsumer();

app.Run();
