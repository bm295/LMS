using LMS.Application;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace WebApplication.Authentication;

public sealed class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration,
    ILogger<BasicAuthenticationHandler> log)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var expectedUsername = configuration[AppSettings.BasicAuthUsername];
        var expectedPassword = configuration[AppSettings.BasicAuthPassword];

        if (string.IsNullOrWhiteSpace(expectedUsername) || string.IsNullOrWhiteSpace(expectedPassword))
        {
            log.LogError("Basic authentication credentials not configured in settings");
            return Task.FromResult(AuthenticateResult.Fail("Server configuration error"));
        }

        if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }

        if (!AuthenticationHeaderValue.TryParse(authHeaderValue, out var headerValue) ||
            !string.Equals(headerValue.Scheme, "Basic", StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(headerValue.Parameter))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header."));
        }

        string credentialString;
        try
        {
            credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter));
        }
        catch (FormatException)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Base64 encoding."));
        }

        var separatorIndex = credentialString.IndexOf(':');
        if (separatorIndex <= 0 || separatorIndex == credentialString.Length - 1)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid basic authentication credentials."));
        }

        var username = credentialString[..separatorIndex];
        var password = credentialString[(separatorIndex + 1)..];

        return ValidateCredentialsAsync(username, password, expectedUsername, expectedPassword)
            .ContinueWith(async task =>
            {
                if (!await task)
                {
                    return AuthenticateResult.Fail("Invalid username or password.");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }, TaskScheduler.Default)
            .Unwrap();
    }

    private static Task<bool> ValidateCredentialsAsync(string username, string password, string expectedUsername, string expectedPassword)
    {
        return Task.FromResult(
            string.Equals(username, expectedUsername, StringComparison.Ordinal) &&
            string.Equals(password, expectedPassword, StringComparison.Ordinal)
        );
    }
}
