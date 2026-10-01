namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

/// <summary>
/// Handles the GetProductByIdQuery to retrieve a product by its ID.
/// </summary>
/// <param name="session"></param>
/// <param name="logger"></param>
internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    /// <summary>
    /// Handles the GetProductByIdQuery to retrieve a product by its ID.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQuery.Handle for Product ID: {@ProductId}", request.Id);

        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product);
    }
}
