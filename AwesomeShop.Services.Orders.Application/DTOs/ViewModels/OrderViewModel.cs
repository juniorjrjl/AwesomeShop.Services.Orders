namespace AwesomeShop.Services.Orders.Application.DTOs.ViewModels;

public record OrderViewModel(Guid Id, decimal Total, DateTime CreatedAt, string Status);