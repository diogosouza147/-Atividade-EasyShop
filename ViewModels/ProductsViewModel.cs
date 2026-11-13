using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using EasyShop.Models;
using EasyShop.Services;

namespace EasyShop.ViewModels;

/// <summary>Camada de apresentação: só chama o serviço, nunca o banco.</summary>
public class ProductsViewModel : BaseViewModel
{
    private readonly IProductService _service;

    private string _name = string.Empty;
    private string _price = string.Empty;
    private string _category = string.Empty;
    private string _attributes = string.Empty;

    public ProductsViewModel(IProductService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        LoadCommand = new Command(async () => await LoadAsync());
    }

    public ObservableCollection<Product> Products { get; } = new();

    public string Name { get => _name; set => Set(ref _name, value); }
    public string Price { get => _price; set => Set(ref _price, value); }
    public string Category { get => _category; set => Set(ref _category, value); }
    public string Attributes { get => _attributes; set => Set(ref _attributes, value); }

    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }

    public async Task LoadAsync()
    {
        try
        {
            var items = await _service.ListAsync();
            Products.Clear();
            foreach (var p in items) Products.Add(p);
        }
        catch (Exception ex) { ShowError($"Erro ao carregar produtos: {ex.Message}"); }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (!TryParsePrice(Price, out var price))
                throw new BusinessException("Informe um preço válido (ex.: 59,90).");

            await _service.CreateAsync(Name, price, Category, Attributes);

            Name = Price = Category = Attributes = string.Empty;
            ShowSuccess("Produto salvo no banco NoSQL (LiteDB).");
            await LoadAsync();
        }
        catch (BusinessException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Erro inesperado: {ex.Message}"); }
    }

    private static bool TryParsePrice(string text, out decimal price) =>
        decimal.TryParse(text?.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out price);
}
