

using MassTransit;
using Microsoft.Extensions.Configuration;
using Payments.Service.Core;
using Payments.Service.FactoryClass;
using Payments.Service.Interface;
using Payments.Service.Momo;
using Payments.Service.Momo.Config;
using Payments.Service.RabbitMQ.Producers;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("paymentService");
            h.Password("<123456Aa.>");
        });
        cfg.ConfigureEndpoints(context);

        cfg.Publish<PaymentSubmitedProducer>(x =>
        {
            x.ExchangeType = "fanout"; // default, allows any valid exchange type
        });
    });

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MomoSetting>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddTransient<PaymentMethod, MomoService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<MomoService>();
builder.Services.AddTransient<PaymentSubmitedProducer>();
var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
