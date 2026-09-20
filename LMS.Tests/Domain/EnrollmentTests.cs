using LMS.Domain.Enrollments;

namespace LMS.Tests.Domain;

public sealed class EnrollmentTests
{
    [Fact]
    public void Create_CalculatesDeadlineUsingCourseDeadlineCalculator()
    {
        var enrollment = Enrollment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateTime(2026, 9, 21),
            standardDays: 5);

        Assert.Equal(new DateTime(2026, 9, 28), enrollment.Deadline);
        Assert.False(enrollment.HasExtension);
        Assert.Null(enrollment.CompletedAt);
    }

    [Fact]
    public void Extend_RecalculatesDeadlineWithSevenAdditionalWorkingDays()
    {
        var enrollment = Enrollment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateTime(2026, 9, 21),
            standardDays: 5);

        enrollment.Extend();

        Assert.True(enrollment.HasExtension);
        Assert.Equal(new DateTime(2026, 10, 7), enrollment.Deadline);
    }

    [Fact]
    public void Extend_IsIdempotent()
    {
        var enrollment = Enrollment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateTime(2026, 9, 21),
            standardDays: 5);

        enrollment.Extend();
        var deadlineAfterFirstExtension = enrollment.Deadline;

        enrollment.Extend();

        Assert.Equal(deadlineAfterFirstExtension, enrollment.Deadline);
    }
}
