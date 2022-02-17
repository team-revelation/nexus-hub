using Contracts.Database.Firestore;
using Contracts.Memorystore;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Database
{
    public static class Database
    {
        public static FirestoreDb Store;
    }
    
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services)
        {
            Database.Store = FirestoreDb.Create("nexus-server-f82c8");
            
            services.AddMemorystoreService();
            services.AddScoped<IFirestoreService, FirestoreService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
        }
    }
}