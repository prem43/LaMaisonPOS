using LaMaisonPOS.Models;

namespace LaMaisonPOS.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        Product? GetProductByCode(string code);
    }
}
