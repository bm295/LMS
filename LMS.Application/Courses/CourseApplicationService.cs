namespace LMS.Application.Courses;

public sealed class CourseApplicationService(
    IListPublishedCoursesUseCase listPublishedCoursesUseCase) : ICourseApplicationService
{
    public Task<IReadOnlyCollection<CourseSummary>> ListPublishedAsync(
        CancellationToken cancellationToken = default)
    {
        return listPublishedCoursesUseCase.ExecuteAsync(cancellationToken);
    }
}
