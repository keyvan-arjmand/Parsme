using System.Text.Json;
using Application.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDatabase _database;

    public ResponseCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task CacheResponseAsync(string cacheKey, object? response)
    {
        if (response is null)
            return;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedResponse = JsonSerializer.Serialize(response, options);

        await _database.StringSetAsync(cacheKey, serializedResponse, TimeSpan.FromMinutes(2)).ConfigureAwait(false);
    }

    public async Task<string?> GetCachedResponseAsync(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey).ConfigureAwait(false);

        if (cachedResponse.IsNullOrEmpty)
            return null;

        return cachedResponse;
    }

    public async Task<List<T>> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<List<T>>> getDataFunc  )
    {
        var cachedData = await GetCachedResponseAsync(cacheKey);
        List<T> data;
        if (!string.IsNullOrEmpty(cachedData))
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            data = JsonSerializer.Deserialize<List<T>>(cachedData, options);
        }
        else
        {
            data = await getDataFunc();
            await CacheResponseAsync(cacheKey, data );
        }
        return data;
    }

    public async Task<T> GetOrSetSingleCacheAsync<T>(string cacheKey, Func<Task<T>> getSingleDataFunc)
    {
        var cachedData = await GetCachedResponseAsync(cacheKey);
        T data;

        if (!string.IsNullOrEmpty(cachedData))
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            data = JsonSerializer.Deserialize<T>(cachedData, options);
        }
        else
        {
            data = await getSingleDataFunc();
            await CacheResponseAsync(cacheKey, data);
        }

        return data;
    }
}