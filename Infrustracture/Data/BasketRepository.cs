using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrustracture.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _dataBase;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _dataBase.KeyDeleteAsync(basketId.ToString());
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _dataBase.StringGetAsync(basketId.ToString());
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _dataBase.StringSetAsync(basket.Id.ToString(),
            JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetBasketAsync(basket.Id);

        }
    }
}