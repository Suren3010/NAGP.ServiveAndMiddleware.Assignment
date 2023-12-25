using Microsoft.OpenApi.Models;
using NAGP.Product.Service;
using NAGP.Product.Service.Controllers;
using NAGP.Product.Service.Services;
using NAGP.Product.Service.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services
    .AddSingleton<IProductOrderService, ProductOrderService>()
    .AddGrpcSwagger()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo { Title = "Product service", Version = "v1" });
    })
    .AddGrpcClient<Orders.OrdersClient>("OrderClient", grpcClient =>
    {
        grpcClient.Address = new Uri("https://localhost:7199");
    });
    

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product service");
});
// Configure the HTTP request pipeline.
app.MapGrpcService<ProductController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
