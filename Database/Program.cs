using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Database
{
    public class Program
    {
        public static FirestoreDb Store;

        public static void Main(string[] args)
        {
            InitializeFirebase();
            CreateHostBuilder(args).Build().Run();
        }
        
        private static void InitializeFirebase()
        {
            Store = FirestoreDb.Create("nexus-server-f82c8");
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                                          {
                                              var port = Environment.GetEnvironmentVariable("PORT");
                    
                                              webBuilder.UseStartup<Startup>()
                                                        .UseUrls("http://*:" + port, "http://127.0.0.1:8080");
                                          });
    }
}