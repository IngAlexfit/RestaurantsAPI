using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurants_From_Colombia.BD;
using Restaurants_From_Colombia.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure your services and settings
builder.Services.AddControllers();
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

var mongoDBSettings = configuration.GetSection("ConnectionStrings:MongoDB").Value;
builder.Services.AddSingleton(new MongoDBSettings { MongoDBConnection = mongoDBSettings });
builder.Services.AddScoped<RestauranteService>();

// Add Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}/");

app.Run();
