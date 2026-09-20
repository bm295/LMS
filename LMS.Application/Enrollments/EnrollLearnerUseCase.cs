using LMS.Application.Abstractions;
using LMS.Domain.Enrollments;

namespace LMS.Application.Enrollments;

public sealed class EnrollLearnerUseCase(
    IEnrollmentRepository enrollmentRepository) : IEnrollLearnerUseCase
{
    public async Task<EnrollLearnerResponse> ExecuteAsync(
        EnrollLearnerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (await enrollmentRepository.ExistsAsync(request.UserId, request.CourseId, cancellationToken))
        {
            throw new InvalidOperationException("The learner is already enrolled in this course.");
        }

        var enrollment = Enrollment.Create(
            request.EnrollmentId,
            request.UserId,
            request.CourseId,
            request.StartDate,
            request.StandardDays,
            request.HasExtension);

        await enrollmentRepository.AddAsync(enrollment, cancellationToken);

        return new EnrollLearnerResponse(
            enrollment.Id,
            enrollment.UserId,
            enrollment.CourseId,
            enrollment.StartDate,
            enrollment.Deadline,
            enrollment.HasExtension);
    }
}
