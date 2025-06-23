using MLZ2025.Core.Model;
using MLZ2025.Core.ViewModel;

namespace MLZ2025.Core.Services
{
    public static class CoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<MainPage>()
                .AddSingleton<MainViewModel>()
                .AddTransient<DetailPage>()
                .AddTransient<DetailViewModel>()
                .AddSingleton(Connectivity.Current)
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<DataAccess<DatabaseAddress>>()
                .AddSingleton<DataAccessSettings>()
                .AddTransient<HttpClient>();
        }
    }
}
