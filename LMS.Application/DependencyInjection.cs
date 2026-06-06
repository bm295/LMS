using LMS.Application.Courses;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IListPublishedCoursesUseCase, ListPublishedCoursesUseCase>();

        return services;
    }
}
