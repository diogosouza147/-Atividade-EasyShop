using EasyShop.Models;
using SQLite;

namespace EasyShop.Data;

/// <summary>Camada de dados SQL: só fala com o SQLite.</summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly Task _init;

    public CustomerRepository(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _init = _db.CreateTableAsync<Customer>(); // cria a tabela se não existir
    }

    public async Task<int> AddAsync(Customer customer)
    {
        await _init;
        return await _db.InsertAsync(customer);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        await _init;
        return await _db.Table<Customer>().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        await _init;
        var count = await _db.Table<Customer>().Where(c => c.Email == email).CountAsync();
        return count > 0;
    }
}
