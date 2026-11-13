using SQLite;

namespace EasyShop.Models;

/// <summary>Entidade relacional (SQLite): tabela "Customer".</summary>
public class Customer
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    // Índice único: o próprio banco impede e-mails duplicados.
    [NotNull, Unique, MaxLength(150)]
    public string Email { get; set; } = string.Empty;
}
