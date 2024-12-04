namespace Application.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
    Task<List<T>> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<List<T>>> getDataFunc, TimeSpan expiration);
    Task<T> GetOrSetSingleCacheAsync<T>(string cacheKey, Func<Task<T>> getDataSingleFunc, TimeSpan expiration);

}