using LMS.Application.Courses;
using LMS.Application.Enrollments;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IListPublishedCoursesUseCase, ListPublishedCoursesUseCase>();
        services.AddScoped<ICourseApplicationService, CourseApplicationService>();
        services.AddScoped<IEnrollLearnerUseCase, EnrollLearnerUseCase>();
        services.AddScoped<IEnrollmentApplicationService, EnrollmentApplicationService>();

        return services;
    }
}
