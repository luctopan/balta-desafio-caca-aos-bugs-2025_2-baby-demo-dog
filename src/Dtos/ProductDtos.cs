namespace BugStore.Dtos;

public record ProductCreateDto(string Title, string Description, string Slug, decimal Price);
public record ProductUpdateDto(string Title, string Description, string Slug, decimal Price);
public record ProductReadDto(Guid Id, string Title, string Description, string Slug, decimal Price);