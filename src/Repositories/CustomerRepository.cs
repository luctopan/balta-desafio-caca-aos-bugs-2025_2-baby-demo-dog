using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface ICustomerRepository: IRepository<Customer> {}

public sealed class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Customer>> GetAllAsync(CancellationToken ct = default) =>
        await context.Customers.AsNoTracking().ToListAsync(ct);

    public async Task<Customer> AddAsync(Customer entity, CancellationToken ct = default)
    {
        context.Customers.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<Customer> UpdateAsync(Customer entity, CancellationToken ct = default)
    {
        context.Customers.Update(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return;
        context.Customers.Remove(entity);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Customer> Query() => context.Customers.AsNoTracking();
}