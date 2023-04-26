using System.Diagnostics.CodeAnalysis;
using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.Yandex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Artisan.Server;

/// <summary>
/// Provides methods for adding OAuth authorization.
/// </summary>
public static class OAuth
{
    public static AuthenticationBuilder AddCustomOAuth(this AuthenticationBuilder auth, IConfiguration config)
    {
        IConfigurationSection authConfig = config.GetSection("Authentication");
        if (authConfig.Exists() is false) return auth;

        TryAddOAuth<GoogleOptions>(authConfig, "Google", auth.AddGoogle);
        TryAddOAuth<YandexAuthenticationOptions>(authConfig, "Yandex", auth.AddYandex);
        TryAddOAuth<DiscordAuthenticationOptions>(authConfig, "Discord", auth.AddDiscord);

        return auth;
    }
    
    private static void TryAddOAuth<TOptions>(
        IConfiguration authConfig, 
        string name,
        Func<Action<TOptions>, AuthenticationBuilder> addOAuthDelegate)
        where TOptions : OAuthOptions
    {
        if (TryGetConfiguration(authConfig, name, out var clientId, out var clientSecret) is false) 
            return;
        
        string callbackPath = $"/signin-{name.ToLower()}";
        
        addOAuthDelegate(options =>
        {
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.CallbackPath = callbackPath;
        });
    }

    private static bool TryGetConfiguration(
        IConfiguration config, 
        string section, 
        [NotNullWhen(true)] out string? clientId, 
        [NotNullWhen(true)] out string? clientSecret)
    {
        IConfiguration oauthCfg = config.GetSection(section);
        
        clientId = oauthCfg["ClientId"];
        clientSecret = oauthCfg["ClientSecret"];

        return 
            string.IsNullOrWhiteSpace(clientId) is false && 
            string.IsNullOrWhiteSpace(clientSecret) is false;
    }
}