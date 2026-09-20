namespace LMS.Application.Courses;

public interface ICourseApplicationService
{
    Task<IReadOnlyCollection<CourseSummary>> ListPublishedAsync(
        CancellationToken cancellationToken = default);
}
