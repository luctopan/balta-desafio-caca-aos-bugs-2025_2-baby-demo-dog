using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class OrderEndpoints
{
    private record CreateOrderRequest(Guid CustomerId, List<OrderLineCreateDto> Lines);

    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/orders");

        group.MapGet("/{id:guid}", async ([FromServices] IOrderService service, Guid id, CancellationToken ct) =>
        {
            var result = await service.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] IOrderService service, [FromBody] CreateOrderRequest order, CancellationToken ct) =>
        {
            var created = await service.CreateAsync(order.CustomerId, order.Lines, ct);
            return Results.Created($"/v1/orders/{created.Id}", created);
        });

        return app;
    }
}