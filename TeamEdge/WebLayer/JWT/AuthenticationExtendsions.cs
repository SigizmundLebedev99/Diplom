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
                options.SecurityTokenValidators.Add(new GoogleTokenValidator());
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidIssuer = AuthTokenOptions.ISSUER,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    IssuerSigningKey = AuthTokenOptions.GetSymmetricSecurityKey(),

                    ValidateIssuerSigningKey = true
                };
            }).AddOAuth("GitHub", options =>
            {
                options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                options.CallbackPath = new PathString("/signin-github");

                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(JwtRegisteredClaimNames.GivenName, "name");
                options.ClaimActions.MapJsonKey(JwtRegisteredClaimNames.Email, "email");
                options.ClaimActions.MapJsonKey(JwtRegisteredClaimNames.UniqueName, "login");
                options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        context.RunClaimActions(user);
                    }
                };
            }).AddGoogle(options =>
            {
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                options.ClientId = Configuration["Authentication:Google:ClientId"];
            });

            return services;
        }
    }
}
