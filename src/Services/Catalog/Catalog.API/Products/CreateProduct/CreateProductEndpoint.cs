namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the routes for the CreateProduct endpoint to the application.
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // convert the request to a command using Mapster
            // send the command to the handler using MediatR
            // convert the result to a response using Mapster
            // return a 201 response with the location of the new product
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var reponse = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{reponse.Id}", reponse);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new product")
        .WithDescription("Creates a new product in the catalog.");
    }
}
