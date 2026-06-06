namespace LMS.Domain.Organizations;

public sealed class Organization
{
    private Organization(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    public static Organization Create(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Organization id must be provided.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Organization name must be provided.", nameof(name));
        }

        return new Organization(id, name.Trim());
    }
}
