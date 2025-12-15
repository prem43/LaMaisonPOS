using LaMaisonPOS.Models;

namespace LaMaisonPOS.Services
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        void AddToCart(Product product, int quantity = 1);
        void UpdateQuantity(int productId, int quantity);
        void RemoveFromCart(int productId);
        void ClearCart();
        decimal GetSubtotal();
        int GetTotalItems();
    }
}
