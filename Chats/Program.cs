using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Chats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                                          {
                                              var port = Environment.GetEnvironmentVariable("PORT");

                                              webBuilder.UseContentRoot("/");
                                              webBuilder.UseStartup<Startup>()
                                                        .UseUrls("http://*:" + port, "http://127.0.0.1:8080");
                                          });
    }
}