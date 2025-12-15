using Microsoft.AspNetCore.Mvc;
using LaMaisonPOS.Models;
using LaMaisonPOS.Models.ViewModels;
using LaMaisonPOS.Services;

namespace LaMaisonPOS.Controllers
{
    public class POSController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public POSController(IProductService productService, ICartService cartService, IOrderService orderService)
        {
            _productService = productService;
            _cartService = cartService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var model = GetPOSViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddToCart(product, quantity);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Suspend(string customerName, decimal taxAmount, decimal discountAmount)
        {
            var cartItems = _cartService.GetCartItems();
            if (cartItems.Any())
            {
                var orderId = _orderService.CreateOrder(customerName ?? "Walk-in Customer", cartItems, taxAmount, discountAmount);
                _orderService.SuspendOrder(orderId, "Order suspended by user");
                _cartService.ClearCart();
                TempData["Message"] = "Order suspended successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Cancel()
        {
            _cartService.ClearCart();
            TempData["Message"] = "Order cancelled!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ProcessPayment(string customerName, decimal taxAmount, decimal discountAmount, decimal cashReceived)
        {
            var cartItems = _cartService.GetCartItems();
            if (!cartItems.Any())
            {
                TempData["Error"] = "Cart is empty!";
                return RedirectToAction("Index");
            }

            var orderId = _orderService.CreateOrder(customerName ?? "Walk-in Customer", cartItems, taxAmount, discountAmount);
            _orderService.CompleteOrder(orderId);

            var order = _orderService.GetOrder(orderId);
            if (order == null)
            {
                TempData["Error"] = "Error processing order!";
                return RedirectToAction("Index");
            }

            var billModel = new BillViewModel
            {
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                Items = order.Items,
                Subtotal = order.Subtotal,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                CashReceived = cashReceived,
                Change = cashReceived - order.TotalAmount
            };

            _cartService.ClearCart();

            return View("Bill", billModel);
        }

        [HttpGet]
        public IActionResult SearchProduct(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var product = _productService.GetProductByCode(searchTerm);
            if (product != null)
            {
                _cartService.AddToCart(product);
            }
            else
            {
                TempData["Error"] = "Product not found!";
            }

            return RedirectToAction("Index");
        }

        private POSViewModel GetPOSViewModel()
        {
            var cartItems = _cartService.GetCartItems();
            var subtotal = _cartService.GetSubtotal();
            var taxAmount = 0m; // 0% tax as per requirement
            var discountAmount = 0m;

            return new POSViewModel
            {
                Products = _productService.GetAllProducts(),
                CartItems = cartItems,
                CustomerName = "Walk-in Customer",
                Subtotal = subtotal,
                TaxAmount = taxAmount,
                TaxPercentage = 0,
                DiscountAmount = discountAmount,
                TotalPayable = subtotal + taxAmount - discountAmount,
                TotalItems = _cartService.GetTotalItems()
            };
        }
    }
}
