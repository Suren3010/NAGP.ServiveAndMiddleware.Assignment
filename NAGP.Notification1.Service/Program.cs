using NAGP.Notification1.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
var rabbitMQConfigurations = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQConfigurations>();
var rebbitMqConnection = new ConnectionFactory
{
    HostName = rabbitMQConfigurations.HostName
}.CreateConnection();
var channel = rebbitMqConnection.CreateChannel();

var queueName = "fanout-queue-notification1";
string ORDER_EXCHANGE_FANOUT = "order-exchange-fanout";
channel.ExchangeDeclare(exchange: ORDER_EXCHANGE_FANOUT, type: ExchangeType.Fanout);
channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, ORDER_EXCHANGE_FANOUT, "Order.Create", null);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body;
    var messagee = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine($"Received message ---> {messagee}");
};
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


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
