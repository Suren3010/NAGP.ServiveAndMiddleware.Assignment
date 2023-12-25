using NAGP.Notification2.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var rabbitMQConfigurations = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQConfigurations>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//**********************************************************
//*** Rabit mq message listener configurations
//**********************************************************
var rebbitMqConnection = new ConnectionFactory
{
    HostName = rabbitMQConfigurations.HostName
}.CreateConnection();
var channel = rebbitMqConnection.CreateChannel();
var queueName = "fanout-queue-notification2";
string ORDER_EXCHANGE_FANOUT = "order-exchange-fanout";
string ORDER_EXCHANGE_TOPIC = "order-exchange-topic";
channel.ExchangeDeclare(exchange: ORDER_EXCHANGE_FANOUT, type: ExchangeType.Fanout);
channel.ExchangeDeclare(exchange: ORDER_EXCHANGE_TOPIC, type: ExchangeType.Topic);
channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, ORDER_EXCHANGE_FANOUT, "Order.*", null);
channel.QueueBind(queueName, ORDER_EXCHANGE_TOPIC, "Order.*", null);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body;
    var messagee = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine($"Received message ---> {messagee}");
};
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
