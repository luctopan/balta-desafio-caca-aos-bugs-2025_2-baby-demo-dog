using BugStore.Dtos;
using BugStore.Models;
using BugStore.Repositories;

namespace BugStore.Services;

public interface IProductService
{
    Task<ProductReadDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<ProductReadDto>> ListAsync(CancellationToken ct = default);
    Task<ProductReadDto> CreateAsync(ProductCreateDto dto, CancellationToken ct = default);
    Task<ProductReadDto?> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed class ProductService(IProductRepository repository) : IProductService
{
    public async Task<ProductReadDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repository.GetAsync(id, ct);
        if (entity is null) return null;
        
        return new ProductReadDto(entity.Id, entity.Title, entity.Description, entity.Slug, entity.Price);
    }

    public async Task<List<ProductReadDto>> ListAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        
        return list.Select(e => new ProductReadDto(e.Id, e.Title, e.Description, e.Slug, e.Price)).ToList();
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto, CancellationToken ct = default)
    {
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Slug = dto.Slug,
            Price = dto.Price
        };

        await repository.AddAsync(entity, ct);
        
        return new ProductReadDto(entity.Id, entity.Title, entity.Description, entity.Slug, entity.Price);
    }

    public async Task<ProductReadDto?> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetAsync(id, ct);
        if (entity is null) return null;

        entity.Title = dto.Title;
        entity.Description = dto.Description;
        entity.Slug = dto.Slug;
        entity.Price = dto.Price;

        await repository.UpdateAsync(entity, ct);
        
        return new ProductReadDto(entity.Id, entity.Title, entity.Description, entity.Slug, entity.Price);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
        => repository.DeleteAsync(id, ct);
}
