using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Data.DataAccess;
using Orders.Service.Core;
using Orders.Service.gRPC;
using Orders.Service.RabbitMQ.Consumers;
using Orders.Service.RabbitMQ.Messages;
using Orders.Service.RabbitMQ.Producers;
using Products.Service.RabbitMQ.Consumer;
using ProductService.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ProductMainService>();
builder.Services.AddDbContext<OrderDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentSubmitedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("orderService");
            h.Password("<123456Aa.>");
        });
        cfg.ReceiveEndpoint("order", ep =>
        {
            ep.ConfigureConsumer<PaymentSubmitedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
        
        cfg.Publish<OrderCreateProducer>(x =>
        {

            x.ExchangeType = "fanout"; // default, allows any valid exchange type
        });
    });
    
});
builder.Services.AddScoped<OrderCreateProducer>();
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
