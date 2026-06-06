namespace LMS.Application.Courses;

public interface IListPublishedCoursesUseCase
{
    Task<IReadOnlyCollection<CourseSummary>> ExecuteAsync(CancellationToken cancellationToken = default);
}
