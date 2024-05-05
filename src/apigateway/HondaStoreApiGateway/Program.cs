using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Users.Service.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//1
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                                .AddJsonFile("ocelot.json",optional: false, reloadOnChange: true);

//2. Add service to the container
builder.Services.AddOcelot();

builder.Services.AddCustomJwtAuthentication();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseHttpsRedirection();


app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();
//3
app.UseOcelot().Wait();
app.UseAuthorization();
app.UseAuthentication();
app.Run();
