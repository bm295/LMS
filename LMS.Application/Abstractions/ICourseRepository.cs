using LMS.Domain.Courses;

namespace LMS.Application.Abstractions;

public interface ICourseRepository
{
    Task<IReadOnlyCollection<Course>> ListPublishedAsync(CancellationToken cancellationToken = default);
}
