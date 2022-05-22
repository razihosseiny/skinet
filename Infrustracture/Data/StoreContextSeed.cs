using System.Text.Json;
using API;
using Microsoft.Extensions.Logging;
using Core.Entities;

namespace Infrustracture.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory LoggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrustracture/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.ProductTypes.Any())
                {
                    var productTypeData = File.ReadAllText("../Infrustracture/Data/SeedData/types.json");
                    var prodctTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                    foreach (var item in prodctTypes)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any())
                {
                    var productData = File.ReadAllText("../Infrustracture/Data/SeedData/ProductList.json");
                    var prodcts = JsonSerializer.Deserialize<List<Product>>(productData);
                    foreach (var item in prodcts)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.ProductPrices.Any())
                {
                    var productPriceData = File.ReadAllText("../Infrustracture/Data/SeedData/ProductPrice.json");
                    var prodctPrices = JsonSerializer.Deserialize<List<ProductPrice>>(productPriceData);
                    foreach (var item in prodctPrices)
                    {
                        context.ProductPrices.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.ProductPics.Any())
                {
                    var productPicData = File.ReadAllText("../Infrustracture/Data/SeedData/ProductPic.json");
                    var prodctPices = JsonSerializer.Deserialize<List<ProductPic>>(productPicData);
                    foreach (var item in prodctPices)
                    {
                        context.ProductPics.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}