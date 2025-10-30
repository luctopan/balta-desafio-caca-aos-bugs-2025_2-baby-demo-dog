namespace BugStore.Dtos;

public record OrderLineCreateDto(Guid ProductId, int Quantity);
public record OrderReadDto(Guid Id, Guid CustomerId, DateTime CreatedAt, DateTime UpdatedAt, List<OrderLineReadDto> Lines);
public record OrderLineReadDto(Guid Id, Guid ProductId, string ProductTitle, int Quantity, decimal Total);