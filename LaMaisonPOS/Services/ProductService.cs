using LaMaisonPOS.Models;
using LaMaisonPOS.Repositories;

namespace LaMaisonPOS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts() => _productRepository.GetAll();

        public Product? GetProductById(int id) => _productRepository.GetById(id);

        public Product? GetProductByCode(string code) => _productRepository.GetByCode(code);
    }
}
