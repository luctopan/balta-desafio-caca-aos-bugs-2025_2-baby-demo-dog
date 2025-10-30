using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class OrderEndpoints
{
    public record CreateOrderRequest(Guid CustomerId, List<OrderLineCreateDto> Lines);

    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/orders");

        group.MapGet("/{id:guid}", async ([FromServices] IOrderService svc, Guid id, CancellationToken ct) =>
        {
            var result = await svc.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] IOrderService svc, [FromBody] CreateOrderRequest req, CancellationToken ct) =>
        {
            var created = await svc.CreateAsync(req.CustomerId, req.Lines, ct);
            return Results.Created($"/v1/orders/{created.Id}", created);
        });

        return app;
    }
}