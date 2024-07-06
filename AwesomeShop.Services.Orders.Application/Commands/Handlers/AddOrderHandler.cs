using AwesomeShop.Services.Orders.Application.Mappers;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure;
using MediatR;
using AwesomeShop.Services.Orders.Application.Integrations;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(
    IOrderRepository repository, 
    IOrderMapper mapper, 
    IMessageBusClient messageBus, 
    ICustomerClient customerClient) : IRequestHandler<AddOrder, Guid>
{

    private readonly IOrderRepository _repository = repository;
    private readonly IOrderMapper _mapper = mapper;
    private readonly IMessageBusClient _messageBus = messageBus;
    private readonly ICustomerClient _customerClient = customerClient;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
    {                   
        var entity = _mapper.ToOrder(request);

        var response = await _customerClient.GetByIdAsync(entity.Customer.Id);
        Console.WriteLine($"response da api {response}");

        await _repository.AddAsync(entity);
        foreach (var @event in entity.Events){
            var routingKey = @event.GetType().Name.ToDashCase();
            _messageBus.Publish(@event, routingKey, "order-service");
        }
        return entity.Id;
    }
}
