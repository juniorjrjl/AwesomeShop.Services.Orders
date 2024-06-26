using AwesomeShop.Services.Orders.Application.DTOs.ViewModels;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries;

public record GetOrderById(Guid Id) : IRequest<OrderViewModel>;