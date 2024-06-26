using AwesomeShop.Services.Orders.Application.Mappers;
using AwesomeShop.Services.Orders.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(IOrderRepository repository, IOrderMapper mapper) : IRequestHandler<AddOrder, Guid>
{

    private readonly IOrderRepository _repository = repository;
    private readonly IOrderMapper _mapper = mapper;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
    {                   
        var entity = _mapper.ToOrder(request);
        await _repository.AddAsync(entity);
        return entity.Id;
    }
}
