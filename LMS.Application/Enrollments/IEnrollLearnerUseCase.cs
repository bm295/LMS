namespace LMS.Application.Enrollments;

public interface IEnrollLearnerUseCase
{
    Task<EnrollLearnerResponse> ExecuteAsync(
        EnrollLearnerRequest request,
        CancellationToken cancellationToken = default);
}
