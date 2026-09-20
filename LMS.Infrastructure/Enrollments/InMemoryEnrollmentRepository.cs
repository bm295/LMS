using System.Collections.Concurrent;
using LMS.Application.Abstractions;
using LMS.Domain.Enrollments;

namespace LMS.Infrastructure.Enrollments;

public sealed class InMemoryEnrollmentRepository : IEnrollmentRepository
{
    private readonly ConcurrentDictionary<Guid, Enrollment> _enrollments = new();

    public Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(enrollment);

        if (!_enrollments.TryAdd(enrollment.Id, enrollment))
        {
            throw new InvalidOperationException("An enrollment with the same id already exists.");
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_enrollments.Values.Any(
            enrollment => enrollment.UserId == userId && enrollment.CourseId == courseId));
    }
}
