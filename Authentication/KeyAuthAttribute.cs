using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class KeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string KeyHeaderName = "Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(KeyHeaderName, out var potentialKey))
                throw new AuthenticationException("Key not found.");

            var key = Environment.GetEnvironmentVariable("NEXUS_API_KEY");
            if (key == null) throw new NullReferenceException("Match key not found.");

            if (!key.Equals(potentialKey))
                throw new AuthenticationException("Invalid key.");
            
            await next();
        }
    }
}