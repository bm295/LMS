using LMS.Application.Abstractions;
using LMS.Infrastructure.Courses;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();

        return services;
    }
}
