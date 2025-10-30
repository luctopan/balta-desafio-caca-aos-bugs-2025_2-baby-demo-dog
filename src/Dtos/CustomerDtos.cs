namespace BugStore.Dtos;

public record CustomerCreateDto(string Name, string Email, string Phone, DateTime BirthDate);
public record CustomerUpdateDto(string Name, string Email, string Phone, DateTime BirthDate);
public record CustomerReadDto(Guid Id, string Name, string Email, string Phone, DateTime BirthDate);