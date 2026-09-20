using LMS.Application.Abstractions;
using LMS.Infrastructure.Courses;
using LMS.Infrastructure.Enrollments;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        services.AddSingleton<IEnrollmentRepository, InMemoryEnrollmentRepository>();

        return services;
    }
}
