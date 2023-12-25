using Microsoft.OpenApi.Models;
using NAGP.Orders.Service.Controllers;
using NAGP.Orders.Service.Models;
using NAGP.Orders.Service.Services;
using NAGP.Orders.Service.Services.Interfaces;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
var rabbitMQConfigurations = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQConfigurations>();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "gRPC order service", Version = "v1" });
});
builder.Services
    .AddSingleton<IOrderService, OrderService>()
    .AddSingleton(new ConnectionFactory
    {
        HostName = rabbitMQConfigurations.HostName
    }.CreateConnection())
    .AddSingleton<IRabitMQProducer, RabitMQProducer>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order service");
});
// Configure the HTTP request pipeline.
app.MapGrpcService<OrderController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
