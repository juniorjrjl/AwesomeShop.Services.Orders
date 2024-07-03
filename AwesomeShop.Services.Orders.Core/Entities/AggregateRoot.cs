
using AwesomeShop.Services.Orders.Core.Events;

namespace AwesomeShop.Services.Orders.Core.Entities;

public class AggregateRoot : IEntityBase
{
    private List<IDomainEvent> _events = [];
    public Guid Id { get; set; }

    public IEnumerable<IDomainEvent> Events => _events;

    protected void AddEvent(IDomainEvent @event){
        _events ??= [];
        _events.Add(@event);
    }

}