using System;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Notifications
{
    public static class ServiceCollectionExtensions
    {
        private static void Initialize()
        {
            try { Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"); } 
            catch (Exception e) { throw new NullReferenceException("Google credentials not set."); }
            
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
        }
        
        public static void AddNotificationService(this IServiceCollection services)
        {
            if (FirebaseAuth.DefaultInstance == null)
                Initialize();
            
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}