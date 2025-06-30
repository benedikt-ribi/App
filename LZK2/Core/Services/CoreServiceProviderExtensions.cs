using Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services;

public static class CoreServiceProviderExtensions
{
    public static IServiceCollection CreateDefaultServiceCollection()
    {
        return new ServiceCollection().AddDefaultServices();
    }

    public static IServiceCollection AddDefaultServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ILocalStorage<Person>, SqliteLocalStorage<Person>>()
            .AddSingleton<LocalStorageSettings>()
            .AddSingleton<Person>()
            .AddTransient<IPersonService, PersonService>() // Registrierung des neuen Dienstes
            .AddTransient<MainPageViewModel>();
    }

    public static IServiceProvider CreateDefaultServiceProvider()
    {
        return CreateDefaultServiceCollection().BuildServiceProvider();
    }
}