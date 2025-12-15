using LaMaisonPOS.Models;

namespace LaMaisonPOS.Services
{
    public interface IOrderService
    {
        int CreateOrder(string customerName, List<CartItem> items, decimal taxAmount, decimal discountAmount);
        Order? GetOrder(int orderId);
        void SuspendOrder(int orderId, string reason);
        void CancelOrder(int orderId);
        void CompleteOrder(int orderId);
        List<Order> GetSuspendedOrders();
    }
}
