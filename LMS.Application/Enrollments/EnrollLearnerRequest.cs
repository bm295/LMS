namespace LMS.Application.Enrollments;

public sealed record EnrollLearnerRequest(
    Guid EnrollmentId,
    Guid UserId,
    Guid CourseId,
    DateTime StartDate,
    int StandardDays,
    bool HasExtension = false);
