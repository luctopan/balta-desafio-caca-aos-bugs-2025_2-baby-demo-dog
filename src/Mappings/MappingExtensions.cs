using BugStore.Dtos;
using BugStore.Models;

namespace BugStore.Mappings;

public static class MappingExtensions
{
    public static CustomerReadDto ToReadDto(this Customer customer) =>
        new(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);

    public static ProductReadDto ToReadDto(this Product product) =>
        new(product.Id, product.Title, product.Description, product.Slug, product.Price);

    private static OrderLineReadDto ToReadDto(this OrderLine orderLine) =>
        new(orderLine.Id, orderLine.ProductId, orderLine.Product?.Title ?? string.Empty, orderLine.Quantity, orderLine.Total);

    public static OrderReadDto ToReadDto(this Order order) =>
        new(order.Id, order.CustomerId, order.CreatedAt, order.UpdatedAt, order.Lines.Select(l => l.ToReadDto()).ToList());
}