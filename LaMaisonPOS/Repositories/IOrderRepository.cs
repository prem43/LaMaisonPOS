using LaMaisonPOS.Models;

namespace LaMaisonPOS.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order? GetById(int id);
        Order? GetByOrderNumber(string orderNumber);
        int Add(Order order);
        void Update(Order order);
        List<Order> GetSuspendedOrders();
    }
}
