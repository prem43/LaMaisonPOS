using LaMaisonPOS.Models;
using LaMaisonPOS.Repositories;

namespace LaMaisonPOS.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public int CreateOrder(string customerName, List<CartItem> items, decimal taxAmount, decimal discountAmount)
        {
            var order = new Order
            {
                CustomerName = customerName,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Active,
                Items = items.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    ProductCode = c.ProductCode,
                    ProductName = c.ProductName,
                    Price = c.Price,
                    Quantity = c.Quantity
                }).ToList()
            };

            order.Subtotal = order.Items.Sum(i => i.Subtotal);
            order.TaxAmount = taxAmount;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = order.Subtotal + order.TaxAmount - order.DiscountAmount;

            return _orderRepository.Add(order);
        }

        public Order? GetOrder(int orderId) => _orderRepository.GetById(orderId);

        public void SuspendOrder(int orderId, string reason)
        {
            var order = _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Suspended;
                order.SuspendReason = reason;
                _orderRepository.Update(order);
            }
        }

        public void CancelOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                _orderRepository.Update(order);
            }
        }

        public void CompleteOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                _orderRepository.Update(order);
            }
        }

        public List<Order> GetSuspendedOrders() => _orderRepository.GetSuspendedOrders();
    }
}
