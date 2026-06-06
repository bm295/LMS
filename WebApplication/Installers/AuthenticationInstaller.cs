using LMS.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Authentication;

namespace WebApplication.Installers;

public sealed class AuthenticationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var tenantId = configuration[AppSettings.TenantId];
        var authority = configuration[AppSettings.Authority] ?? throw new InvalidOperationException($"Missing configuration value: {AppSettings.Authority}");
        var audience = configuration[AppSettings.Audience] ?? throw new InvalidOperationException($"Missing configuration value: {AppSettings.Audience}");
        var authorityWithTenant = string.IsNullOrWhiteSpace(tenantId)
            ? authority
            : authority.TrimEnd('/') + "/" + tenantId.Trim('/');
        var validIssuers = configuration.GetSection(AppSettings.ValidIssuers).Get<string[]>()
            ?? new[] { authorityWithTenant };

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authorityWithTenant;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuers = validIssuers
                };
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", options => { });
    }
}
