using LMS.Application.Abstractions;
using LMS.Domain.Courses;

namespace LMS.Infrastructure.Courses;

public sealed class InMemoryCourseRepository : ICourseRepository
{
    private static readonly Guid OrganizationId = Guid.Parse("b1f50a35-ef8f-4ab7-a615-6979076dc97b");

    private static readonly IReadOnlyCollection<Course> SeedCourses = CreateSeedCourses();

    public Task<IReadOnlyCollection<Course>> ListPublishedAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var publishedCourses = SeedCourses
            .Where(course => course.Status == CourseStatus.Published)
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<Course>>(publishedCourses);
    }

    private static IReadOnlyCollection<Course> CreateSeedCourses()
    {
        var onboarding = Course.Draft(
            Guid.Parse("75ed4c5d-113d-46a1-bf84-5322b8c82aa1"),
            OrganizationId,
            "LMS Foundations",
            "A guided introduction to the platform, roles, and learning workflows.");
        onboarding.AddLesson(Lesson.Create(Guid.Parse("57035ea6-0997-495a-86e9-2d89cb124cd7"), "Navigate the learner workspace", 1));
        onboarding.AddLesson(Lesson.Create(Guid.Parse("a595fda1-2654-41a5-98f3-5ac53433542c"), "Complete a course module", 2));
        onboarding.Publish();

        var authoring = Course.Draft(
            Guid.Parse("af09f31e-a3ff-4da8-a52d-80e5d4e58f25"),
            OrganizationId,
            "Course Authoring Essentials",
            "Learn how instructors structure content, publish lessons, and prepare assessments.");
        authoring.AddLesson(Lesson.Create(Guid.Parse("4a06931c-62a1-423e-aa87-149557a0c2a1"), "Design measurable learning outcomes", 1));
        authoring.AddLesson(Lesson.Create(Guid.Parse("78e8920e-e3ce-4956-9f62-6bfadf00a915"), "Publish a course safely", 2));
        authoring.Publish();

        return [onboarding, authoring];
    }
}
