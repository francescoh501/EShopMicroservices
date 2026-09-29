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
            var command = request.Adapt<CreateProductCommand>();
            // send the command to the handler using MediatR
            var result = await sender.Send(command);
            // convert the result to a response using Mapster
            var reponse = result.Adapt<CreateProductResponse>();
            // return a 201 Created response with the location of the new product
            return Results.Created($"/products/{reponse.Id}", reponse);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new product")
        .WithDescription("Creates a new product in the catalog.");
    }
}
