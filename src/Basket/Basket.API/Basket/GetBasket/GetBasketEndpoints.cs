namespace Basket.API.Basket.GetBasket;

public record GetBasketResponce(ShoppingCart Cart);
public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            var respons = result.Adapt<GetBasketResponce>();
            return Results.Ok(respons);
        })
            .WithName("GetBasket")
            .Produces<GetBasketResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");

    }
}
