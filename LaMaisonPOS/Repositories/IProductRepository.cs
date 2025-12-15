using LaMaisonPOS.Models;

namespace LaMaisonPOS.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        Product? GetByCode(string code);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
