using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using Project_API.Repositories;
using Serilog;
using System.Text.Json.Serialization;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateBootstrapLogger();


try
{

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Starting up");
    //add serilog

    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<MyDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Api-Project"));
    });

    builder.Services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
    builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
    builder.Services.AddScoped<IDishRepository, DishRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
    builder.Services.AddScoped<IRiderRepository, RiderRepository>();

    //added extra
    builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    builder.Services.AddAutoMapper(typeof(Program).Assembly);



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

    app.UseSerilogRequestLogging();

    app.Run();

}
catch(Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


