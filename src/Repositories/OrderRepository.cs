using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface IOrderRepository : IRepository<Order> {}

public sealed class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context
            .Orders
            .Include(o => o.Lines)!.ThenInclude(l => l.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Order>> GetAllAsync(CancellationToken ct = default) =>
        await context
            .Orders
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync(ct);
        
        return order;
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken ct = default)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync(ct);
        
        return order;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (order is null)
            return;
        
        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Order> Query() => context.Orders.AsNoTracking();
}