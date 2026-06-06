namespace LMS.Application.Courses;

public sealed record CourseSummary(
    Guid Id,
    Guid OrganizationId,
    string Title,
    string Description,
    string Status,
    IReadOnlyCollection<LessonSummary> Lessons);
