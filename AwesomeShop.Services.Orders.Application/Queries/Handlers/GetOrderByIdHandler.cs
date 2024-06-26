using AwesomeShop.Services.Orders.Application.DTOs.ViewModels;
using AwesomeShop.Services.Orders.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries.Handlers;

public class GetOrderByIdHandler(IOrderRepository repository) : IRequestHandler<GetOrderById, OrderViewModel>
{
    private readonly IOrderRepository _repository = repository;

    public async Task<OrderViewModel> Handle(GetOrderById request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        return new OrderViewModel(entity.Id, entity.Total, entity.CreatedAt, entity.Status.ToString());
    }
}
