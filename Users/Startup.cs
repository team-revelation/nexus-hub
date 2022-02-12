using System.Reflection;
using Contracts.Chats;
using Contracts.Database;
using Contracts.Exploring;
using Contracts.Redis;
using Contracts.Users;
using MediatR;
using MediatR.Behaviors.Authorization.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Users
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
            services.AddExploreService();
            services.AddChatService();
            
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddMediatorAuthorization(Assembly.GetExecutingAssembly());
            services.AddAuthorizersFromAssembly(Assembly.GetExecutingAssembly());
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}