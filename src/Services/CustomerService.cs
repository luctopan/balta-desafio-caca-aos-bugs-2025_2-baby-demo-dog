using BugStore.Dtos;
using BugStore.Models;
using BugStore.Repositories;

namespace BugStore.Services;

public interface ICustomerService
{
    Task<CustomerReadDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<CustomerReadDto>> ListAsync(CancellationToken ct = default);
    Task<CustomerReadDto> CreateAsync(CustomerCreateDto dto, CancellationToken ct = default);
    Task<CustomerReadDto?> UpdateAsync(Guid id, CustomerUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<CustomerReadDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repository.GetAsync(id, ct);
        if (entity is null) return null;
        
        return new CustomerReadDto(entity.Id, entity.Name, entity.Email, entity.Phone, entity.BirthDate);
    }

    public async Task<List<CustomerReadDto>> ListAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        
        return list.Select(e => new CustomerReadDto(e.Id, e.Name, e.Email, e.Phone, e.BirthDate)).ToList();
    }

    public async Task<CustomerReadDto> CreateAsync(CustomerCreateDto dto, CancellationToken ct = default)
    {
        var entity = new Customer
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            BirthDate = dto.BirthDate
        };

        await repository.AddAsync(entity, ct);
        
        return new CustomerReadDto(entity.Id, entity.Name, entity.Email, entity.Phone, entity.BirthDate);
    }

    public async Task<CustomerReadDto?> UpdateAsync(Guid id, CustomerUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetAsync(id, ct);
        if (entity is null) return null;

        entity.Name = dto.Name;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.BirthDate = dto.BirthDate;

        await repository.UpdateAsync(entity, ct);
        
        return new CustomerReadDto(entity.Id, entity.Name, entity.Email, entity.Phone, entity.BirthDate);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
        => repository.DeleteAsync(id, ct);
}
