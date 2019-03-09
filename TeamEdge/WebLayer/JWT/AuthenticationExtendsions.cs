using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TeamEdge.JWT
{
    public static class AuthenticationExtendsions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidIssuer = AuthTokenOptions.ISSUER,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    IssuerSigningKey = AuthTokenOptions.GetSymmetricSecurityKey(),

                    ValidateIssuerSigningKey = true
                };
            });

            services.ConfigureApplicationCookie(options =>
            options.Events.OnRedirectToLogin =
            context => {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            });
            return services;
        }
    }
}
