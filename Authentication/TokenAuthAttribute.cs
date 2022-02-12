using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthAttribute : Attribute, IAsyncActionFilter
    {
        public static string CurrentEmail { get; private set; }

        private void Initialize()
        {
            try { Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"); } 
            catch (Exception e) { throw new NullReferenceException("Google credentials not set."); }
            
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var potentialToken))
                throw new AuthenticationException("Token not found");
            
            try
            {
                if (FirebaseAuth.DefaultInstance == null)
                    Initialize();
                
                var pureToken = potentialToken.ToString().Replace("Bearer ", "");
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(pureToken);
                var user = await FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid);
                
                CurrentEmail = user.Email;
            }   
            catch (FirebaseAuthException e)
            {
                throw new AuthenticationException(e.AuthErrorCode.ToString());
            }

            await next();
        }
    }
}