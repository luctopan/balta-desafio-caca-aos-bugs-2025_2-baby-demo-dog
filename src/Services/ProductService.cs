using BugStore.Dtos;
using BugStore.Models;
using BugStore.Repositories;

namespace BugStore.Services;

public interface IProductService
{
    Task<ProductReadDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<ProductReadDto>> ListAsync(CancellationToken ct = default);
    Task<ProductReadDto> CreateAsync(ProductCreateDto product, CancellationToken ct = default);
    Task<ProductReadDto?> UpdateAsync(Guid id, ProductUpdateDto product, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed class ProductService(IProductRepository repository) : IProductService
{
    public async Task<ProductReadDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var product = await repository.GetAsync(id, ct);
        return product is null
            ? null
            : new ProductReadDto(product.Id, product.Title, product.Description, product.Slug, product.Price);
    }

    public async Task<List<ProductReadDto>> ListAsync(CancellationToken ct = default)
    {
        var products = await repository.GetAllAsync(ct);
        
        return products.Select(product => new ProductReadDto(product.Id, product.Title, product.Description, product.Slug, product.Price)).ToList();
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto model, CancellationToken ct = default)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Slug = model.Slug,
            Price = model.Price
        };

        await repository.AddAsync(product, ct);
        
        return new ProductReadDto(product.Id, product.Title, product.Description, product.Slug, product.Price);
    }

    public async Task<ProductReadDto?> UpdateAsync(Guid id, ProductUpdateDto model, CancellationToken ct = default)
    {
        var product = await repository.GetAsync(id, ct);
        
        if (product is null) return null;

        product.Title = model.Title;
        product.Description = model.Description;
        product.Slug = model.Slug;
        product.Price = model.Price;

        await repository.UpdateAsync(product, ct);
        
        return new ProductReadDto(product.Id, product.Title, product.Description, product.Slug, product.Price);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
        => repository.DeleteAsync(id, ct);
}
