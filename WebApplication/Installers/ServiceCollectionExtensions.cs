using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.Installers;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Scans the assembly containing the marker type for all IInstaller implementations
    /// and invokes their InstallServices method.
    /// </summary>
    public static IServiceCollection InstallServicesInAssembly<TMarker>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TMarker : class
    {
        var assembly = typeof(TMarker).Assembly;
        var installerType = typeof(IInstaller);

        var installers = assembly
            .GetTypes()
            .Where(t => installerType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();

        foreach (var installer in installers)
        {
            installer.InstallServices(services, configuration);
        }

        return services;
    }
}
