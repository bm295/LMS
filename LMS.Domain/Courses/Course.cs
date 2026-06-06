namespace LMS.Domain.Courses;

public sealed class Course
{
    private readonly List<Lesson> _lessons = [];

    private Course(Guid id, Guid organizationId, string title, string description, CourseStatus status)
    {
        Id = id;
        OrganizationId = organizationId;
        Title = title;
        Description = description;
        Status = status;
    }

    public Guid Id { get; }

    public Guid OrganizationId { get; }

    public string Title { get; }

    public string Description { get; }

    public CourseStatus Status { get; private set; }

    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    public static Course Draft(Guid id, Guid organizationId, string title, string description)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Course id must be provided.", nameof(id));
        }

        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("Organization id must be provided.", nameof(organizationId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Course title must be provided.", nameof(title));
        }

        return new Course(id, organizationId, title.Trim(), description?.Trim() ?? string.Empty, CourseStatus.Draft);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (_lessons.Any(existing => existing.Sequence == lesson.Sequence))
        {
            throw new InvalidOperationException($"A lesson already exists at sequence {lesson.Sequence}.");
        }

        _lessons.Add(lesson);
    }

    public void Publish()
    {
        if (_lessons.Count == 0)
        {
            throw new InvalidOperationException("A course must contain at least one lesson before publishing.");
        }

        Status = CourseStatus.Published;
    }
}
