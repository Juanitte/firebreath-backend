using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Services
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, TimeSpan? expiry = null);
        Task RemoveAsync(string key);
        Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, TimeSpan? lockTtl = null);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _redis;

        public RedisCacheService(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(data);
            if (expiry.HasValue)
            {
                // Usa la sobrecarga que recibe TTL
                await _redis.StringSetAsync(key, json, expiry.Value);
            }
            else
            {
                // Usa la sobrecarga sin TTL (persistente)
                await _redis.StringSetAsync(key, json);
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }

        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, TimeSpan? lockTtl = null)
        {
            // 1. Leer de cache
            var cached = await GetAsync<T>(key);
            if (cached != null)
                return cached;

            var lockKey = $"lock:{key}";
            TimeSpan? lockTime = lockTtl;

            // 2. Intentar obtener el lock
            var lockTaken = await _redis.StringSetAsync(lockKey, "1", lockTime, when: When.NotExists);
            if (lockTaken)
            {
                try
                {
                    // Ejecutar la función para obtener el dato
                    var value = await factory();
                    if (value != null)
                        await SetAsync(key, value, ttl);
                    return value;
                }
                finally
                {
                    await _redis.KeyDeleteAsync(lockKey);
                }
            }
            else
            {
                // 3. Esperar y reintentar una vez
                await Task.Delay(100);
                return await GetAsync<T>(key);
            }
        }
    }
}
