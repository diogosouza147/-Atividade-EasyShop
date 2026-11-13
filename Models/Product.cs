using LiteDB;

namespace EasyShop.Models;

/// <summary>
/// Documento NoSQL (LiteDB/BSON): coleção "products".
/// "Attributes" é um dicionário livre, então cada produto pode ter
/// características diferentes sem alterar nenhum schema.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new();

    // Propriedades apenas para exibição (não são gravadas no documento).
    [BsonIgnore]
    public string PriceText => Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

    [BsonIgnore]
    public string CategoryText => string.IsNullOrWhiteSpace(Category) ? "Sem categoria" : Category!;

    [BsonIgnore]
    public string AttributesText => Attributes.Count == 0
        ? "Sem atributos"
        : string.Join(" • ", Attributes.Select(a => $"{a.Key}: {a.Value}"));
}
