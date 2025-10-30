using BugStore.Dtos;
using BugStore.Models;

namespace BugStore.Mappings;

public static class MappingExtensions
{
    public static CustomerReadDto ToReadDto(this Customer customer) =>
        new(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);

    public static ProductReadDto ToReadDto(this Product product) =>
        new(product.Id, product.Title, product.Description, product.Slug, product.Price);

    public static OrderLineReadDto ToReadDto(this OrderLine orderLine) =>
        new(orderLine.Id, orderLine.ProductId, orderLine.Product?.Title ?? string.Empty, orderLine.Quantity, orderLine.Total);

    public static OrderReadDto ToReadDto(this Order o) =>
        new(o.Id, o.CustomerId, o.CreatedAt, o.UpdatedAt, o.Lines.Select(l => l.ToReadDto()).ToList());
}