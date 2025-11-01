using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/products");

        group.MapGet("/", async ([FromServices] IProductService service, CancellationToken ct) =>
        {
            var result = await service.ListAsync(ct);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async ([FromServices] IProductService service, Guid id, CancellationToken ct) =>
        {
            var result = await service.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] IProductService service, [FromBody] ProductCreateDto product, CancellationToken ct) =>
        {
            var created = await service.CreateAsync(product, ct);
            return Results.Created($"/v1/products/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async ([FromServices] IProductService service, Guid id, [FromBody] ProductUpdateDto product, CancellationToken ct) =>
        {
            var updated = await service.UpdateAsync(id, product, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async ([FromServices] IProductService service, Guid id, CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        return app;
    }
}