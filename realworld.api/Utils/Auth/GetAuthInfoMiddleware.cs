using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Realworld.Api.Utils.Auth
{
    /// <summary>
    /// Should be called after Authenticate, must have constructor with HttpContext as first param,
    /// But only Authenticated succeeded (have token), Invokes wwill be called, which add claims to User (through SignInAsync), without Authorize attr, this is still called but failed authResult
    /// </summary>
    public class GetAuthInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public GetAuthInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (authResult.Succeeded) {
                var props = authResult.Properties; //prop have Items.Token have access token and expirty date
                context.User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Country, "Vietnam"));
                context.SignInAsync(JwtBearerDefaults.AuthenticationScheme, authResult.Principal, authResult.Properties);
            }

            await _next(context);
        }
    }
}