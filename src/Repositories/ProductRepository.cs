using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface IProductRepository : IRepository<Product> {}

public sealed class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Product>> GetAllAsync(CancellationToken ct = default) =>
        await context.Products.AsNoTracking().ToListAsync(ct);

    public async Task<Product> AddAsync(Product entity, CancellationToken ct = default)
    {
        context.Products.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<Product> UpdateAsync(Product entity, CancellationToken ct = default)
    {
        context.Products.Update(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return;
        context.Products.Remove(entity);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Product> Query() => context.Products.AsNoTracking();
}