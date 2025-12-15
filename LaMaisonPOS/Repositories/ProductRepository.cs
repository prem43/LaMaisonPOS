using LaMaisonPOS.Models;

namespace LaMaisonPOS.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private int _nextId = 1;

        public ProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = _nextId++, Code = "AD118", Name = "PEARL AND PAPER PLANE EARINGS", Price = 2300.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR001", Name = "Diamond Necklace", Price = 15000.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR002", Name = "Gold Bracelet", Price = 8500.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR003", Name = "Silver Ring", Price = 3200.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR004", Name = "Pearl Earrings", Price = 4500.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR005", Name = "Ruby Pendant", Price = 12000.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR006", Name = "Emerald Brooch", Price = 9800.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR007", Name = "Platinum Chain", Price = 18500.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR008", Name = "Sapphire Ring", Price = 11200.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR009", Name = "Crystal Bracelet", Price = 5600.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR010", Name = "Rose Gold Anklet", Price = 6700.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR011", Name = "Titanium Watch", Price = 21000.00m, IsActive = true },
                new Product { Id = _nextId++, Code = "PR012", Name = "Amethyst Earrings", Price = 4200.00m, IsActive = true }
            };
        }

        public List<Product> GetAll() => _products.Where(p => p.IsActive).ToList();

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product? GetByCode(string code) => _products.FirstOrDefault(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        public void Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Code = product.Code;
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.ImageUrl = product.ImageUrl;
                existing.IsActive = product.IsActive;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                product.IsActive = false;
            }
        }
    }
}
