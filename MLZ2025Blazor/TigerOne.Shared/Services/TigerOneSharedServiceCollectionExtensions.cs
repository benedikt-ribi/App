using Microsoft.Extensions.DependencyInjection;
using MLZ2025.Shared.Services;

namespace TigerOne.Shared.Services
{
    public static class TigerOneSharedServiceCollectionExtensions
    {
        public static IServiceCollection AddTigerOneSharedServices(this IServiceCollection services)
        {
            return services
                .AddSharedServices();
        }
    }
}
