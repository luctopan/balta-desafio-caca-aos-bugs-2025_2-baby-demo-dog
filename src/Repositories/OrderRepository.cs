using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface IOrderRepository : IRepository<Order> {}

public sealed class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context.Orders
            .Include(o => o.Lines)!.ThenInclude(l => l.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Order>> GetAllAsync(CancellationToken ct = default) =>
        await context.Orders.AsNoTracking().ToListAsync(ct);

    public async Task<Order> AddAsync(Order entity, CancellationToken ct = default)
    {
        context.Orders.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<Order> UpdateAsync(Order entity, CancellationToken ct = default)
    {
        context.Orders.Update(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return;
        context.Orders.Remove(entity);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Order> Query() => context.Orders.AsNoTracking();
}