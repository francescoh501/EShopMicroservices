namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

/// <summary>
/// Handles the GetProductByIdQuery to retrieve a product by its ID.
/// </summary>
/// <param name="session"></param>
/// <param name="logger"></param>
internal class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    /// <summary>
    /// Handles the GetProductByIdQuery to retrieve a product by its ID.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        return product is null ? throw new ProductNotFoundException(query.Id) : new GetProductByIdResult(product);
    }
}
