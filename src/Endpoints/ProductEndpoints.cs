using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/products");

        group.MapGet("/", async ([FromServices] IProductService svc, CancellationToken ct) =>
        {
            var result = await svc.ListAsync(ct);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async ([FromServices] IProductService svc, Guid id, CancellationToken ct) =>
        {
            var result = await svc.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] IProductService svc, [FromBody] ProductCreateDto dto, CancellationToken ct) =>
        {
            var created = await svc.CreateAsync(dto, ct);
            return Results.Created($"/v1/products/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async ([FromServices] IProductService svc, Guid id, [FromBody] ProductUpdateDto dto, CancellationToken ct) =>
        {
            var updated = await svc.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async ([FromServices] IProductService svc, Guid id, CancellationToken ct) =>
        {
            await svc.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        return app;
    }
}