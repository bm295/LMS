using LMS.Application.Abstractions;
using LMS.Application.Enrollments;
using LMS.Domain.Enrollments;

namespace LMS.Tests.Application;

public sealed class EnrollLearnerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_CreatesEnrollmentWithCalculatedDeadline()
    {
        var repository = new StubEnrollmentRepository();
        var useCase = new EnrollLearnerUseCase(repository);
        var request = new EnrollLearnerRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateTime(2026, 9, 21),
            StandardDays: 5);

        var result = await useCase.ExecuteAsync(request);

        Assert.Equal(request.CourseId, result.CourseId);
        Assert.Equal(new DateTime(2026, 9, 28), result.Deadline);
        Assert.NotNull(repository.SavedEnrollment);
    }

    [Fact]
    public async Task ExecuteAsync_RejectsDuplicateEnrollment()
    {
        var repository = new StubEnrollmentRepository();
        var useCase = new EnrollLearnerUseCase(repository);
        var request = new EnrollLearnerRequest(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Today, 5);

        await useCase.ExecuteAsync(request);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(request with { EnrollmentId = Guid.NewGuid() }));

        Assert.Equal("The learner is already enrolled in this course.", exception.Message);
    }

    private sealed class StubEnrollmentRepository : IEnrollmentRepository
    {
        public Enrollment? SavedEnrollment { get; private set; }

        public Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            SavedEnrollment = enrollment;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(SavedEnrollment is not null &&
                SavedEnrollment.UserId == userId && SavedEnrollment.CourseId == courseId);
        }
    }
}
