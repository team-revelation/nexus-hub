using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Chats
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChatService(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
        }
    }
}