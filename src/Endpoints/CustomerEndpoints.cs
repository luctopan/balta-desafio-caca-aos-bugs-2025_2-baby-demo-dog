using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/customers");

        group.MapGet("/", async ([FromServices] ICustomerService svc, CancellationToken ct) =>
        {
            var result = await svc.ListAsync(ct);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async ([FromServices] ICustomerService svc, Guid id, CancellationToken ct) =>
        {
            var result = await svc.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] ICustomerService svc, [FromBody] CustomerCreateDto dto, CancellationToken ct) =>
        {
            var created = await svc.CreateAsync(dto, ct);
            return Results.Created($"/v1/customers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async ([FromServices] ICustomerService svc, Guid id, [FromBody] CustomerUpdateDto dto, CancellationToken ct) =>
        {
            var updated = await svc.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async ([FromServices] ICustomerService svc, Guid id, CancellationToken ct) =>
        {
            await svc.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        return app;
    }
}