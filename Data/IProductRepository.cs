using EasyShop.Models;

namespace EasyShop.Data;

public interface IProductRepository
{
    Task<int> AddAsync(Product product);
    Task<List<Product>> GetAllAsync();
}
