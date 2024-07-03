using AwesomeShop.Services.Orders.Application.Mappers;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(IOrderRepository repository, IOrderMapper mapper, IMessageBusClient messageBus) : IRequestHandler<AddOrder, Guid>
{

    private readonly IOrderRepository _repository = repository;
    private readonly IOrderMapper _mapper = mapper;
    private readonly IMessageBusClient _messageBus = messageBus;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
    {                   
        var entity = _mapper.ToOrder(request);
        await _repository.AddAsync(entity);
        foreach (var @event in entity.Events){
            var routingKey = @event.GetType().Name.ToDashCase();
            _messageBus.Publish(@event, routingKey, "order-service");
        }
        return entity.Id;
    }
}
