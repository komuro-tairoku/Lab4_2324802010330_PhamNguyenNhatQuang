using Messaging.Common.Publishing;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// RabbitMQ Connection
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest"
    };

    return factory.CreateConnection();
});


// RabbitMQ Channel
builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    var channel = connection.CreateModel();

    // Declare exchange
    channel.ExchangeDeclare(
        exchange: "ecommerce_exchange",
        type: ExchangeType.Topic,
        durable: true
    );

    return channel;
});


// Register Publisher
builder.Services.AddSingleton<Publisher>();


var app = builder.Build();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();