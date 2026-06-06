namespace LMS.Domain.Courses;

public sealed class Lesson
{
    private Lesson(Guid id, string title, int sequence)
    {
        Id = id;
        Title = title;
        Sequence = sequence;
    }

    public Guid Id { get; }

    public string Title { get; }

    public int Sequence { get; }

    public static Lesson Create(Guid id, string title, int sequence)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Lesson id must be provided.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Lesson title must be provided.", nameof(title));
        }

        if (sequence <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), sequence, "Lesson sequence must be positive.");
        }

        return new Lesson(id, title.Trim(), sequence);
    }
}
