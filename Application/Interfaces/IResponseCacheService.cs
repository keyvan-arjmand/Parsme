namespace Application.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object? response);
    Task<string?> GetCachedResponseAsync(string cacheKey);
    Task<List<T>> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<List<T>>> getDataFunc );
    Task<T> GetOrSetSingleCacheAsync<T>(string cacheKey, Func<Task<T>> getDataSingleFunc );

}