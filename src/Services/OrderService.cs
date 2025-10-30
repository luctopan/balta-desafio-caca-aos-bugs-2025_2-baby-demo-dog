using BugStore.Dtos;
using BugStore.Models;
using BugStore.Repositories;

namespace BugStore.Services;

public interface IOrderService
{
    Task<OrderReadDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<OrderReadDto> CreateAsync(Guid customerId, List<OrderLineCreateDto> lines, CancellationToken ct = default);
}

public sealed class OrderService(IOrderRepository orders, IProductRepository products) : IOrderService
{
    public async Task<OrderReadDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await orders.GetAsync(id, ct);
        if (entity is null) return null;

        return new OrderReadDto(
            entity.Id,
            entity.CustomerId,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Lines.Select(l => new OrderLineReadDto(
                l.Id,
                l.ProductId,
                l.Product?.Title ?? string.Empty,
                l.Quantity,
                l.Total
            )).ToList()
        );
    }

    public async Task<OrderReadDto> CreateAsync(Guid customerId, List<OrderLineCreateDto> lines, CancellationToken ct = default)
    {
        if (lines is null || lines.Count == 0)
            throw new ArgumentException("Order must have at least one line");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = new()
        };

        foreach (var line in lines)
        {
            var prod = await products.GetAsync(line.ProductId, ct)
                       ?? throw new InvalidOperationException($"Product {line.ProductId} not found");

            order.Lines.Add(new OrderLine
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = prod.Id,
                Quantity = line.Quantity,
                Total = prod.Price * line.Quantity
            });
        }

        await orders.AddAsync(order, ct);
        
        var saved = await orders.GetAsync(order.Id, ct) ?? order;

        return new OrderReadDto(
            saved.Id,
            saved.CustomerId,
            saved.CreatedAt,
            saved.UpdatedAt,
            saved.Lines.Select(l => new OrderLineReadDto(
                l.Id,
                l.ProductId,
                l.Product?.Title ?? string.Empty,
                l.Quantity,
                l.Total
            )).ToList()
        );
    }
}
