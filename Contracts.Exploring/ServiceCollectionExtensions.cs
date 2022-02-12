using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Exploring
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExploreService(this IServiceCollection services)
        {
            services.AddScoped<IExploreService, ExploreService>();
        }
    }
}