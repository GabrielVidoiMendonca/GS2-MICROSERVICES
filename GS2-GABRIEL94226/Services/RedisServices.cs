using StackExchange.Redis;
using Newtonsoft.Json;


namespace GS2_GABRIEL94226.Services
{
    public class RedisServices
    {
        private readonly IDatabase _cache;
        private const int CacheExpirationMinutes = 10;

        public RedisServices()
        {
           
            var connection = ConnectionMultiplexer.Connect("localhost:6379");
            _cache = connection.GetDatabase();
        }

        
        public T GetCache<T>(string key)
        {
            var cachedValue = _cache.StringGet(key);
            if (cachedValue.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(cachedValue);
        }

        public void SetCache<T>(string key, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            _cache.StringSet(key, serializedValue, TimeSpan.FromMinutes(CacheExpirationMinutes));
        }
    }
}