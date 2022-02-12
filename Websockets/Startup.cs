using System;
using Contracts.Chats;
using Contracts.Database;
using Contracts.Redis;
using Contracts.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Websockets.Services;

namespace Websockets
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRedisService();
            services.AddDatabaseService();
            services.AddUserService();
            services.AddChatService();
            services.AddSingleton<ISocketService, SocketService>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(options => options.WithOrigins("http://localhost:5000").AllowAnyHeader().AllowAnyMethod());
            
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                                       .Get<IExceptionHandlerPathFeature>()
                                       .Error.GetBaseException();
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
            
            app.UseRouting();

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            });
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}