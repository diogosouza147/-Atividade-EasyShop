using System.Net.Mail;
using EasyShop.Data;
using EasyShop.Models;

namespace EasyShop.Services;

public interface ICustomerService
{
    Task<Customer> CreateAsync(string fullName, string email);
    Task<List<Customer>> ListAsync();
}

/// <summary>Regras de negócio do cadastro de clientes. Não conhece SQLite nem UI.</summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository) => _repository = repository;

    public async Task<Customer> CreateAsync(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length < 2)
            throw new BusinessException("Informe o nome completo (nome e sobrenome).");

        email = (email ?? string.Empty).Trim().ToLowerInvariant();
        if (!IsValidEmail(email))
            throw new BusinessException("Informe um e-mail válido.");

        if (await _repository.EmailExistsAsync(email))
            throw new BusinessException("Já existe um cliente cadastrado com esse e-mail.");

        var customer = new Customer { Name = fullName.Trim(), Email = email };
        await _repository.AddAsync(customer);
        return customer;
    }

    public Task<List<Customer>> ListAsync() => _repository.GetAllAsync();

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains('.')) return false;
        try { return new MailAddress(email).Address == email; }
        catch (FormatException) { return false; }
    }
}
