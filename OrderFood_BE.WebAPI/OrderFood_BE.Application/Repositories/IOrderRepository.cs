using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Order>> GetOrdersByShopIdAsync(Guid shopId);
        Task<Order> GetOrderWithItemsAsync(Guid orderId);
        Task<Order> GetOrderByFirebaseId(string firebaseId);
    }
}
