namespace LMS.Application.Enrollments;

public sealed record EnrollLearnerResponse(
    Guid EnrollmentId,
    Guid UserId,
    Guid CourseId,
    DateTime StartDate,
    DateTime Deadline,
    bool HasExtension);
