using Microsoft.EntityFrameworkCore;
using API;
using API.Extensions;
using Infrustracture.Data;
using API.Helpers;
using API.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//connectionString
var connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(connectionString));
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"),
    true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

ApplicationServiceExtensions.AddApplicationServices(builder.Services);

SwaggerServiceExtensions.AddSwaggerDocumentation(builder.Services);
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyHeader().WithOrigins("https://localhost:4200");
    });
});

var app = builder.Build();
await ApplyMigrations(app);

// migrate any database changes on startup (includes initial db creation)
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
//     db.Database.Migrate();
// }
// using (var scope = app.Services.CreateScope())
// {
//     var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
//     var dataContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
//     var logger = loggerFactory.CreateLogger<Program>();

//     try
//     {
//         //AutomaticMigrationsEnabled =true;
//         dataContext.Database.Migrate();
//         logger.LogWarning(dataContext.Database.GetMigrations().Count().ToString());
//     }
//     catch (Exception ex)
//     {
//         logger.LogError(ex, "Migrate Failed");
//     }
// }
// static void ApplyMigrations(WebApplication app)
// {
//     using (var scope = app.Services.CreateScope())
//     {
//         var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
//         var logger = loggerFactory.CreateLogger<Program>();

//         try
//         {
//             var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
//             db.Database.Migrate();
//             logger.LogWarning("Migrate Sucess");
//         }
//         catch (Exception ex)
//         {

//             logger.LogError(ex, "An Error Occured During Migration");
//         }
//     }
// }
static async Task ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();

    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    // var logger = loggerFactory.CreateLogger<Program>();
    //db.GetInfrastructure().GetService<IMigrator>();
    try
    {
        db.Database.EnsureCreated();
        await db.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(db, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured during Migration");
    }


    //logger.Log( db.Database.GetMigrations());
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

SwaggerServiceExtensions.UseSwaggerDocumentation(app);

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.UseStaticFiles();

app.Run();
