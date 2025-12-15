namespace LaMaisonPOS.Models.ViewModels
{
    public class POSViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
        public string CustomerName { get; set; } = "Walk-in Customer";
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPayable { get; set; }
        public int TotalItems { get; set; }
    }
}
