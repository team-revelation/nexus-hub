using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Database
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseService, DatabaseService>();
        }
    }
}