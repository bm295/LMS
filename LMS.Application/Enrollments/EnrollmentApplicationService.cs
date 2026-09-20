namespace LMS.Application.Enrollments;

public sealed class EnrollmentApplicationService(
    IEnrollLearnerUseCase enrollLearnerUseCase) : IEnrollmentApplicationService
{
    public Task<EnrollLearnerResponse> EnrollAsync(
        Guid courseId,
        Guid userId,
        DateTime startDate,
        int standardDays,
        bool hasExtension = false,
        CancellationToken cancellationToken = default)
    {
        var request = new EnrollLearnerRequest(
            Guid.NewGuid(),
            userId,
            courseId,
            startDate,
            standardDays,
            hasExtension);

        return enrollLearnerUseCase.ExecuteAsync(request, cancellationToken);
    }
}
