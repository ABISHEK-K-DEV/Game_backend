using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using GameAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod() 
               .AllowAnyHeader() 
               .AllowCredentials());
});


var mongoConnectionString = builder.Configuration.GetSection("MongoDB:ConnectionURI").Value;


if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new ArgumentNullException("MongoDB connection string is not configured.");
}


builder.Services.AddSingleton<IMongoClient>(serviceProvider => new MongoClient(mongoConnectionString));

builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddControllers();

var app = builder.Build();


app.UseCors("AllowLocalhost3000");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();