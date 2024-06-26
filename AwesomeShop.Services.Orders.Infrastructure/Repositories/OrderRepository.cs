using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MongoDB.Driver;

namespace AwesomeShop.Services.Orders.Infrastructure.Repositories;

public class OrderRepository(IMongoDatabase mongoDatabase) : IOrderRepository
{

    private readonly IMongoCollection<Order> _collection = mongoDatabase.GetCollection<Order>("orders");

    public async Task AddAsync(Order order)
    {
        await _collection.InsertOneAsync(order);
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _collection.Find(c => c.Id == id).SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        await _collection.ReplaceOneAsync(c => c.Id == order.Id, order);
    }
}