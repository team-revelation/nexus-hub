using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Users
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}