using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Contracts.Redis
{
    public static class ServiceCollectionExtensions
    {
        private static RedisCredentials GetRedisCredentials()
        {
            return new RedisCredentials(
                Database: Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"), 
                Username: Environment.GetEnvironmentVariable("REDIS_USERNAME"),
                Password: Environment.GetEnvironmentVariable("REDIS_PASSWORD")
            );
        }

        private static void ConfigureKeySpaceEvents(IConnectionMultiplexer multiplexer)
        {
            var server = multiplexer.GetServer(multiplexer.GetEndPoints().Single());
            server.ConfigSet("notify-keyspace-events", "KEe");
        }

        public static void AddRedisService(this IServiceCollection services, RedisCredentials credentials)
        {
            var multiplexer = ConnectionMultiplexer.Connect(
              new ConfigurationOptions{
                  EndPoints = { credentials.Database },
                  User = credentials.Username,
                  Password = credentials.Password,
              }
            );

            ConfigureKeySpaceEvents(multiplexer);

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddScoped<IRedisService, RedisService>();
        }

        public static void AddRedisService(this IServiceCollection services)
        {
            var (database, username, password) = GetRedisCredentials();
            var multiplexer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions{
                    EndPoints = { database },
                    User = username,
                    Password = password,
                }
            );

            ConfigureKeySpaceEvents(multiplexer);

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddScoped<IRedisService, RedisService>();
        }
    }
}