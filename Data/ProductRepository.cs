using EasyShop.Models;
using LiteDB;

namespace EasyShop.Data;

/// <summary>Camada de dados NoSQL: só fala com o LiteDB (documentos BSON).</summary>
public class ProductRepository : IProductRepository, IDisposable
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<Product> _products;

    public ProductRepository(string dbPath)
    {
        // Connection=shared evita travar o arquivo; uma instância única é thread-safe.
        _db = new LiteDatabase($"Filename={dbPath};Connection=shared");
        _products = _db.GetCollection<Product>("products");
    }

    public Task<int> AddAsync(Product product) =>
        Task.Run(() => _products.Insert(product).AsInt32); // Id int é autoincrementado

    public Task<List<Product>> GetAllAsync() =>
        Task.Run(() => _products.FindAll().OrderByDescending(p => p.Id).ToList());

    public void Dispose() => _db.Dispose();
}
