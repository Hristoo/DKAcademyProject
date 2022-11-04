using ThirtStore.Models.Models;

namespace TshirtStore.DL.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> AddOrder(Order order);
        public Task<Order?> DeleteOrder(int orderId);
        public Task<Order?> GetOrderById(int id);
        public Task<Order?> GetOrderByClientId(int id);
        public Task<Order> UpdateOrder(Order order);
    }
}
