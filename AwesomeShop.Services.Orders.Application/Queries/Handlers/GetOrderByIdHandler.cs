using AwesomeShop.Services.Orders.Application.DTOs.ViewModels;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries.Handlers;

public class GetOrderByIdHandler(IOrderRepository repository, ICacheService cacheService) : IRequestHandler<GetOrderById, OrderViewModel>
{
    private readonly IOrderRepository _repository = repository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<OrderViewModel> Handle(GetOrderById request, CancellationToken cancellationToken)
    {
        var cacheKey = request.Id.ToString();
        var viewModel = await _cacheService.GetAsync<OrderViewModel>(cacheKey);
        if (viewModel is null){
            var entity = await _repository.GetByIdAsync(request.Id);
            viewModel = new OrderViewModel(entity.Id, entity.Total, entity.CreatedAt, entity.Status.ToString());
            await _cacheService.SetAsync(cacheKey, viewModel);
        }
        return viewModel;
    }
}
