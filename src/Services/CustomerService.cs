using BugStore.Dtos;
using BugStore.Models;
using BugStore.Repositories;

namespace BugStore.Services;

public interface ICustomerService
{
    Task<CustomerReadDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<CustomerReadDto>> ListAsync(CancellationToken ct = default);
    Task<CustomerReadDto> CreateAsync(CustomerCreateDto customer, CancellationToken ct = default);
    Task<CustomerReadDto?> UpdateAsync(Guid id, CustomerUpdateDto customer, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<CustomerReadDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var customer = await repository.GetAsync(id, ct);
        
        return customer is null
            ? null
            : new CustomerReadDto(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);
    }

    public async Task<List<CustomerReadDto>> ListAsync(CancellationToken ct = default)
    {
        var customers = await repository.GetAllAsync(ct);
        
        return customers.Select(customer => new CustomerReadDto(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate)).ToList();
    }

    public async Task<CustomerReadDto> CreateAsync(CustomerCreateDto model, CancellationToken ct = default)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            BirthDate = model.BirthDate
        };

        await repository.AddAsync(customer, ct);
        
        return new CustomerReadDto(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);
    }

    public async Task<CustomerReadDto?> UpdateAsync(Guid id, CustomerUpdateDto model, CancellationToken ct = default)
    {
        var customer = await repository.GetAsync(id, ct);
        if (customer is null) return null;

        customer.Name = model.Name;
        customer.Email = model.Email;
        customer.Phone = model.Phone;
        customer.BirthDate = model.BirthDate;

        await repository.UpdateAsync(customer, ct);
        
        return new CustomerReadDto(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
        => repository.DeleteAsync(id, ct);
}
