using BugStore.Dtos;
using BugStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/customers");

        group.MapGet("/", async ([FromServices] ICustomerService service, CancellationToken ct) =>
        {
            var result = await service.ListAsync(ct);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async ([FromServices] ICustomerService service, Guid id, CancellationToken ct) =>
        {
            var result = await service.GetAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] ICustomerService service, [FromBody] CustomerCreateDto customer, CancellationToken ct) =>
        {
            var created = await service.CreateAsync(customer, ct);
            return Results.Created($"/v1/customers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async ([FromServices] ICustomerService service, Guid id, [FromBody] CustomerUpdateDto customer, CancellationToken ct) =>
        {
            var updated = await service.UpdateAsync(id, customer, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async ([FromServices] ICustomerService service, Guid id, CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        return app;
    }
}