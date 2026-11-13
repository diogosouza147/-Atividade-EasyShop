using EasyShop.Data;
using EasyShop.Services;
using EasyShop.ViewModels;
using EasyShop.Views;

namespace EasyShop;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Dois bancos locais, dois arquivos, dois paradigmas.
        var sqlitePath = Path.Combine(FileSystem.AppDataDirectory, "customers.db3"); // SQL
        var litedbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db");   // NoSQL

        // Camada de dados
        builder.Services.AddSingleton<ICustomerRepository>(_ => new CustomerRepository(sqlitePath));
        builder.Services.AddSingleton<IProductRepository>(_ => new ProductRepository(litedbPath));

        // Camada de negócio
        builder.Services.AddSingleton<ICustomerService, CustomerService>();
        builder.Services.AddSingleton<IProductService, ProductService>();

        // Camada de apresentação
        builder.Services.AddSingleton<CustomersViewModel>();
        builder.Services.AddSingleton<ProductsViewModel>();
        builder.Services.AddTransient<CustomersPage>();
        builder.Services.AddTransient<ProductsPage>();

        return builder.Build();
    }
}
