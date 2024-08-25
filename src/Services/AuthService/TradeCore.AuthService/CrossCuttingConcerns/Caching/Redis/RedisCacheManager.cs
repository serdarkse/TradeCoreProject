using Newtonsoft.Json;
using StackExchange.Redis;

namespace TradeCore.AuthService.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly ConfigurationOptions _configurationOptions;
        private readonly Lazy<ConnectionMultiplexer> _connection;
        private string _key;
        public RedisCacheManager(IConfiguration _configuration)
        {

            _configurationOptions = new ConfigurationOptions
            {
                Password = _configuration.GetValue<string>("RedisConfig:Password"),
                User = _configuration.GetValue<string>("RedisConfig:User"),
                DefaultDatabase = _configuration.GetValue<int>("RedisConfig:Db"),
                AsyncTimeout =20000 
            };

            EndPointCollection endPoints = new EndPointCollection();
            var hosts = _configuration.GetValue<string>("RedisConfig:Host").Split(",");
            foreach (var item in hosts)
            {
                _configurationOptions.EndPoints.Add(item, _configuration.GetValue<int>("RedisConfig:Port"));
            }

            _connection = new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect(_configurationOptions)
            );
            _key = _configuration.GetValue<string>("RedisConfig:Key");

        }

        private IDatabase GetDatabase()
        {
            return _connection.Value.GetDatabase();
        }

       public async Task<T> Get<T>(string key)
        {
            key = _key + key;
            var db = GetDatabase();
            var result = await db.StringGetAsync(key);
            if (!result.HasValue)
                return default(T);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<object> Get(string key)
        {
            key = _key + key;

            var db = GetDatabase();
            var result = await db.StringGetAsync(key);
            if (!result.HasValue)
                return default(object);
            return JsonConvert.DeserializeObject<object>(result);
        }

        public async Task Add(string key, object data, int duration)
        {
            key = _key + key;

            var db = GetDatabase();
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            await db.StringSetAsync(key, jsonData, TimeSpan.FromMinutes(duration));
        }

        public async Task Add(string key, object data)
        {
            key = _key + key;

            var db = GetDatabase();
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            await db.StringSetAsync(key, jsonData);
        }

        public async Task<bool> IsAdd(string key)
        {
            key = _key + key;

            var db = GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async Task Remove(string key)
        {
            key = _key + key;

            var db = GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task RemoveByPattern(string pattern)
        {
            pattern = _key + pattern;

            var db = GetDatabase();
            var server = _connection.Value.GetServer(_configurationOptions.EndPoints.First());
            var keys = server.Keys(pattern: $"*{pattern}*");
            foreach (var key in keys)
            {
                await db.KeyDeleteAsync(key);
            }
        }

        public async Task Clear()
        {
            var db = GetDatabase();
            var server = _connection.Value.GetServer(_configurationOptions.EndPoints.First());
            await server.FlushDatabaseAsync((int)_configurationOptions.DefaultDatabase);
        }
    }
}
