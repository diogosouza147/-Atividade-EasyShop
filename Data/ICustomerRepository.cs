using EasyShop.Models;

namespace EasyShop.Data;

public interface ICustomerRepository
{
    Task<int> AddAsync(Customer customer);
    Task<List<Customer>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
}
