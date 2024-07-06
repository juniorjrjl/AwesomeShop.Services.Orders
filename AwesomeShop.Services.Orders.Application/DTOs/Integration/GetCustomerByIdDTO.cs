namespace AwesomeShop.Services.Orders.Application.DTOs.Integration;

public record GetCustomerByIdDTO(Guid Id, string FullName, DateTime BirthDate, AddressDTO Address);
