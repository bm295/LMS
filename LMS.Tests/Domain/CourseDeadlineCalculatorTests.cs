using LMS.Domain.Courses;

namespace LMS.Tests.Domain;

public sealed class CourseDeadlineCalculatorTests
{
    [Fact]
    public void Calculate_CountsOnlyWeekdays()
    {
        var startDate = new DateTime(2026, 9, 21); // Monday

        var deadline = CourseDeadlineCalculator.Calculate(startDate, 5, hasExtension: false);

        Assert.Equal(new DateTime(2026, 9, 28), deadline);
    }

    [Fact]
    public void Calculate_SkipsWeekendWhenStartDateIsFriday()
    {
        var startDate = new DateTime(2026, 9, 25); // Friday

        var deadline = CourseDeadlineCalculator.Calculate(startDate, 1, hasExtension: false);

        Assert.Equal(new DateTime(2026, 9, 28), deadline);
    }

    [Fact]
    public void Calculate_AddsSevenWorkingDaysForExtension()
    {
        var startDate = new DateTime(2026, 9, 21); // Monday

        var deadline = CourseDeadlineCalculator.Calculate(startDate, 5, hasExtension: true);

        Assert.Equal(new DateTime(2026, 10, 7), deadline);
    }

    [Fact]
    public void Calculate_RejectsNonPositiveStandardDays()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            CourseDeadlineCalculator.Calculate(DateTime.Now, 0, hasExtension: false));

        Assert.Equal("standardDays", exception.ParamName);
    }
}
