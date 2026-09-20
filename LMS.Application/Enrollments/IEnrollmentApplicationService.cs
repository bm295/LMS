namespace LMS.Application.Enrollments;

public interface IEnrollmentApplicationService
{
    Task<EnrollLearnerResponse> EnrollAsync(
        Guid courseId,
        Guid userId,
        DateTime startDate,
        int standardDays,
        bool hasExtension = false,
        CancellationToken cancellationToken = default);
}
