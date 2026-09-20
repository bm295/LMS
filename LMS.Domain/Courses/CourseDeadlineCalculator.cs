namespace LMS.Domain.Courses;

public static class CourseDeadlineCalculator
{
    public static DateTime Calculate(
        DateTime startDate,
        int standardDays,
        bool hasExtension)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(standardDays);

        var allowedDays = hasExtension ? standardDays + 7 : standardDays;
        var deadline = startDate;
        var countedDays = 0;

        while (countedDays < allowedDays)
        {
            deadline = deadline.AddDays(1);

            if (deadline.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday))
            {
                countedDays++;
            }
        }

        return deadline;
    }
}
