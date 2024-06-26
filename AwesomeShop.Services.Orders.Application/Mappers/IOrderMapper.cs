using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Core.Entities;

namespace AwesomeShop.Services.Orders.Application.Mappers;

public interface IOrderMapper
{
    
    Order ToOrder(AddOrder request);

}