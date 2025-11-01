using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface IProductRepository : IRepository<Product> {}

public sealed class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context
            .Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Product>> GetAllAsync(CancellationToken ct = default) =>
        await context
            .Products
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync(ct);
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync(ct);
        
        return product;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (product is null)
            return;
        
        context.Products.Remove(product);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Product> Query() => context.Products.AsNoTracking();
}