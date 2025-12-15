using LaMaisonPOS.Models;

namespace LaMaisonPOS.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();
        private int _nextId = 1;

        public List<Order> GetAll() => _orders.ToList();

        public Order? GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

        public Order? GetByOrderNumber(string orderNumber) => 
            _orders.FirstOrDefault(o => o.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));

        public int Add(Order order)
        {
            order.Id = _nextId++;
            order.OrderNumber = $"ORD{DateTime.Now:yyyyMMddHHmmss}";
            _orders.Add(order);
            return order.Id;
        }

        public void Update(Order order)
        {
            var existing = GetById(order.Id);
            if (existing != null)
            {
                existing.CustomerName = order.CustomerName;
                existing.Items = order.Items;
                existing.Subtotal = order.Subtotal;
                existing.TaxAmount = order.TaxAmount;
                existing.DiscountAmount = order.DiscountAmount;
                existing.TotalAmount = order.TotalAmount;
                existing.Status = order.Status;
                existing.SuspendReason = order.SuspendReason;
            }
        }

        public List<Order> GetSuspendedOrders() => 
            _orders.Where(o => o.Status == OrderStatus.Suspended).ToList();
    }
}
