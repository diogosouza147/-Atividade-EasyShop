using EasyShop.Data;
using EasyShop.Models;

namespace EasyShop.Services;

public interface IProductService
{
    /// <param name="attributesText">Formato livre: "Cor=Azul; Tamanho=M; Garantia=12 meses"</param>
    Task<Product> CreateAsync(string name, decimal price, string? category, string? attributesText);
    Task<List<Product>> ListAsync();
}

/// <summary>Regras de negócio do catálogo. Não conhece LiteDB nem UI.</summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository) => _repository = repository;

    public async Task<Product> CreateAsync(string name, decimal price, string? category, string? attributesText)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Informe o nome do produto.");
        if (price < 0)
            throw new BusinessException("O preço não pode ser negativo.");

        var product = new Product
        {
            Name = name.Trim(),
            Price = price,
            Category = string.IsNullOrWhiteSpace(category) ? null : category.Trim(),
            Attributes = ParseAttributes(attributesText)
        };

        product.Id = await _repository.AddAsync(product);
        return product;
    }

    public Task<List<Product>> ListAsync() => _repository.GetAllAsync();

    // Converte "Chave=Valor; Chave2=Valor2" (ou uma por linha) em dicionário.
    private static Dictionary<string, string> ParseAttributes(string? text)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(text)) return result;

        var pairs = text.Split(new[] { ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            if (parts.Length != 2)
                throw new BusinessException($"Atributo inválido: \"{pair.Trim()}\". Use o formato chave=valor.");

            var key = parts[0].Trim();
            var value = parts[1].Trim();
            if (key.Length == 0 || value.Length == 0)
                throw new BusinessException($"Atributo inválido: \"{pair.Trim()}\". Use o formato chave=valor.");

            result[key] = value;
        }
        return result;
    }
}
