using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Realtorist.Models.Helpers;
using Realtorist.Models.Settings;
using Realtorist.Services.Abstractions.Providers;
using Realtorist.Web.Admin.Application.Models.Auth;
using Realtorist.Web.Models.Attributes;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Realtorist.Web.Admin.Application.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISettingsProvider _settingsProvider;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly ILogger _logger;

        public AuthMiddleware(RequestDelegate next, ISettingsProvider settingsProvider, IEncryptionProvider encryptionProvider, ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _encryptionProvider = encryptionProvider ?? throw new ArgumentNullException(nameof(encryptionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            _logger.LogDebug($"AuthMiddleware was hit. Endpoint is null?: {endpoint is null}");
            if (endpoint != null && endpoint.Metadata.Any(m => m is RequireAuthorizationAttribute))
            {
                var tokenHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                var tokenQuery = httpContext.Request.Query.ContainsKey("access_token") ? httpContext.Request.Query["access_token"].FirstOrDefault() : null;

                var tokenArg = tokenQuery ?? tokenHeader;
                try 
                {
                    var tokenJson = _encryptionProvider.Decrypt(tokenArg);
                    var token = tokenJson.FromJson<TokenContainer>();
                    var profile = await _settingsProvider.GetSettingAsync<ProfileSettings>(SettingTypes.Profile);
                    var password = await _settingsProvider.GetSettingAsync<PasswordSettings>(SettingTypes.Password);

                    if (token.Email != profile.Email || token.Guid != password.Guid || DateTime.UtcNow > token.ExpirationTimeUtc)
                    {
                        throw new Exception("Wrong token");
                    }
                }
                catch (Exception e)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync($"Failed to authenticate: {e.Message}");
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
