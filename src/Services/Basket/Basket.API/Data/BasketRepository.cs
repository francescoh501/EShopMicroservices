namespace Basket.API.Data;

/// <summary>
/// Represents a repository for managing shopping cart data in the application.
/// </summary>
/// <param name="session"></param>
public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    /// <summary>
    /// Retrieves the shopping cart for a specific user from the database.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BasketNotFoundException"></exception>
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        return basket is null ? throw new BasketNotFoundException(userName) : basket;
    }

    /// <summary>
    /// Stores or updates the shopping cart in the database.
    /// </summary>
    /// <param name="basket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }

    /// <summary>
    /// Deletes the shopping cart for a specific user from the database.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
