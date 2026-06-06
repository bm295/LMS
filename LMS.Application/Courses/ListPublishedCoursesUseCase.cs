using LMS.Application.Abstractions;

namespace LMS.Application.Courses;

public sealed class ListPublishedCoursesUseCase(ICourseRepository courseRepository) : IListPublishedCoursesUseCase
{
    public async Task<IReadOnlyCollection<CourseSummary>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var courses = await courseRepository.ListPublishedAsync(cancellationToken);

        return courses
            .OrderBy(course => course.Title)
            .Select(course => new CourseSummary(
                course.Id,
                course.OrganizationId,
                course.Title,
                course.Description,
                course.Status.ToString(),
                course.Lessons
                    .OrderBy(lesson => lesson.Sequence)
                    .Select(lesson => new LessonSummary(lesson.Id, lesson.Title, lesson.Sequence))
                    .ToArray()))
            .ToArray();
    }
}
