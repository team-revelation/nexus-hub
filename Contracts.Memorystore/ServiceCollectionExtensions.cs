using System;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Contracts.Memorystore
{
    public static class ServiceCollectionExtensions
    {
        private static MemorystoreCredentials GetMemorystoreCredentials()
        {
            return new MemorystoreCredentials(
                Environment.GetEnvironmentVariable("REDISHOST") ?? "localhost",
                Environment.GetEnvironmentVariable("REDISPORT") ?? "6379"
            );
        }
        
        public static void AddMemorystoreService(this IServiceCollection services, MemorystoreCredentials credentials)
        {
            var multiplexer = ConnectionMultiplexer.Connect($"{credentials.Host}:{credentials.Port},allowAdmin=true");

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddScoped<IMemorystoreService, MemorystoreService>();
        }
        
        public static void AddMemorystoreService(this IServiceCollection services)
        {
            var (host, port) = GetMemorystoreCredentials();
            var multiplexer = ConnectionMultiplexer.Connect($"{host}:{port},allowAdmin=true");
            
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddScoped<IMemorystoreService, MemorystoreService>();
        }
    }
}