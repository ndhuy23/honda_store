using AutoMapper;
using MassTransit;
using Products.Service.gRPC.Services;
using Products.Service.GRPC.Protos;
using Products.Service.RabbitMQ.Consumer;
using Products.Services.Core;
using ProductService.Data.DataAccess;
using ProductService.Utils;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreateConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("productService");
            h.Password("<123456Aa.>");
        });
        cfg.ReceiveEndpoint("product", ep =>
        {
            ep.ConfigureConsumer<OrderCreateConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ProductAppDbSetting>(
    builder.Configuration.GetSection("ProductDatabase"));
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddTransient<IProductService, ProductServices>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IColorService, ColorService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IPreferentialService, PreferentialService>();
builder.Services.AddTransient<IStorageService, StorageService>();


builder.Services.AddSingleton<ProductDbContext>();


IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGrpc();
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
app.MapGrpcService<ProductMainServicegRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
