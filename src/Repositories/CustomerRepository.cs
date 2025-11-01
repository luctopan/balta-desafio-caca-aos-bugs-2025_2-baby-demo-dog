using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public interface ICustomerRepository: IRepository<Customer> {}

public sealed class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken ct = default) =>
        await context
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Customer>> GetAllAsync(CancellationToken ct = default) =>
        await context
            .Customers
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Customer> AddAsync(Customer customer, CancellationToken ct = default)
    {
        context.Customers.Add(customer);
        await context.SaveChangesAsync(ct);
        
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer, CancellationToken ct = default)
    {
        context.Customers.Update(customer);
        await context.SaveChangesAsync(ct);
        
        return customer;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (customer is null)
            return;
        
        context.Customers.Remove(customer);
        await context.SaveChangesAsync(ct);
    }

    public IQueryable<Customer> Query() => context.Customers.AsNoTracking();
}