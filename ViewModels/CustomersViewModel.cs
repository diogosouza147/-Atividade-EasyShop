using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyShop.Models;
using EasyShop.Services;

namespace EasyShop.ViewModels;

public class CustomersViewModel : BaseViewModel
{
    private readonly ICustomerService _service;

    private string _name = string.Empty;
    private string _email = string.Empty;

    public CustomersViewModel(ICustomerService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        LoadCommand = new Command(async () => await LoadAsync());
    }

    public ObservableCollection<Customer> Customers { get; } = new();

    public string Name { get => _name; set => Set(ref _name, value); }
    public string Email { get => _email; set => Set(ref _email, value); }

    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }

    public async Task LoadAsync()
    {
        try
        {
            var items = await _service.ListAsync();
            Customers.Clear();
            foreach (var c in items) Customers.Add(c);
        }
        catch (Exception ex) { ShowError($"Erro ao carregar clientes: {ex.Message}"); }
    }

    private async Task SaveAsync()
    {
        try
        {
            await _service.CreateAsync(Name, Email);
            Name = Email = string.Empty;
            ShowSuccess("Cliente salvo no banco SQL (SQLite).");
            await LoadAsync();
        }
        catch (BusinessException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Erro inesperado: {ex.Message}"); }
    }
}
