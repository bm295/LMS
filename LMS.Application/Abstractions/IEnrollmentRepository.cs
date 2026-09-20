using LMS.Domain.Enrollments;

namespace LMS.Application.Abstractions;

public interface IEnrollmentRepository
{
    Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
}
