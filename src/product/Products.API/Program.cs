using AutoMapper;
using Products.Services.Core;
using ProductService.Data.DataAccess;
using ProductService.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ProductAppDbSetting>(
    builder.Configuration.GetSection("ProductDatabase"));
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
try
{
    var dbContext = app.Services.GetRequiredService<ProductDbContext>();
    dbContext.Create();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}
app.Run();
