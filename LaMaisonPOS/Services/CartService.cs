using LaMaisonPOS.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace LaMaisonPOS.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public List<CartItem> GetCartItems()
        {
            var cartJson = Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            Session.SetString(CartSessionKey, cartJson);
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(cart);
            }
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            Session.Remove(CartSessionKey);
        }

        public decimal GetSubtotal()
        {
            var cart = GetCartItems();
            return cart.Sum(c => c.Subtotal);
        }

        public int GetTotalItems()
        {
            var cart = GetCartItems();
            return cart.Sum(c => c.Quantity);
        }
    }
}
