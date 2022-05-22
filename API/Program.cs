using Microsoft.EntityFrameworkCore;
using API;
using Core.Interfaces;
using Infrustracture.Data;
using API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connectionString
var connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(connectionString));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfile));
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
        //db.Database.EnsureCreated();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
