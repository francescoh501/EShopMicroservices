using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.API.Data;

/// <summary>
/// Represents a cached repository for managing shopping cart data in the application. 
/// This repository wraps an underlying IBasketRepository and provides caching functionality using IDistributedCache.
/// </summary>
/// <param name="repository"></param>
/// <param name="cache"></param>
public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    /// <summary>
    /// Retrieves the shopping cart for a given user from the cache if available; 
    /// otherwise, it fetches it from the underlying repository and caches it for future requests.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    /// <summary>
    /// Stores the provided shopping cart in both the underlying repository and the cache.
    /// </summary>
    /// <param name="basket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    /// <summary>
    /// Deletes the shopping cart for a specific user from both the underlying repository and the cache.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }
}
