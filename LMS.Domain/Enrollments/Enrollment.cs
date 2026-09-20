using LMS.Domain.Courses;

namespace LMS.Domain.Enrollments;

public sealed class Enrollment
{
    private Enrollment(
        Guid id,
        Guid userId,
        Guid courseId,
        DateTime startDate,
        int standardDays,
        bool hasExtension,
        DateTime deadline)
    {
        Id = id;
        UserId = userId;
        CourseId = courseId;
        StartDate = startDate;
        StandardDays = standardDays;
        HasExtension = hasExtension;
        Deadline = deadline;
    }

    public Guid Id { get; }

    public Guid UserId { get; }

    public Guid CourseId { get; }

    public DateTime StartDate { get; }

    public int StandardDays { get; }

    public bool HasExtension { get; private set; }

    public DateTime Deadline { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public static Enrollment Create(
        Guid id,
        Guid userId,
        Guid courseId,
        DateTime startDate,
        int standardDays,
        bool hasExtension = false)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Enrollment id must be provided.", nameof(id));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id must be provided.", nameof(userId));
        }

        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id must be provided.", nameof(courseId));
        }

        var deadline = CourseDeadlineCalculator.Calculate(startDate, standardDays, hasExtension);

        return new Enrollment(
            id,
            userId,
            courseId,
            startDate,
            standardDays,
            hasExtension,
            deadline);
    }

    public void Extend()
    {
        if (HasExtension)
        {
            return;
        }

        HasExtension = true;
        Deadline = CourseDeadlineCalculator.Calculate(StartDate, StandardDays, hasExtension: true);
    }

    public void Complete(DateTime completedAt)
    {
        if (completedAt < StartDate)
        {
            throw new ArgumentException("Completion date cannot be before the start date.", nameof(completedAt));
        }

        CompletedAt = completedAt;
    }
}
