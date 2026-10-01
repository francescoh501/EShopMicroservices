namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    /// <summary>
    /// Handles the GetProductsQuery and returns a GetProductsResult containing the list of products.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        // Retrieve the list of products from the database using the session
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        
        // Return the result containing the list of products
        return new GetProductsResult(products);
    }
}
