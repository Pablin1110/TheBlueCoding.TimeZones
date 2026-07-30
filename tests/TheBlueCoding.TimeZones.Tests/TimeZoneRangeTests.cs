namespace TheBlueCoding.TimeZones.Tests;

public class TimeZoneRangeTests
{
    [Fact]
    public void FromInclusiveLocalDates_ConvertsGuayaquilDayToUtc()
    {
        DateOnly date = new(2026, 7, 30);

        UtcDateRange result = TimeZoneRange.FromInclusiveLocalDates(
            date,
            date,
            "America/Guayaquil");

        Assert.Equal(
            new DateTime(2026, 7, 30, 5, 0, 0, DateTimeKind.Utc),
            result.StartUtc);

        Assert.Equal(
            new DateTime(2026, 7, 31, 5, 0, 0, DateTimeKind.Utc),
            result.EndUtc);
    }

    [Fact]
    public void FromInclusiveLocalDates_HandlesDaylightSavingTransition()
    {
        DateOnly date = new(2026, 3, 8);

        UtcDateRange result = TimeZoneRange.FromInclusiveLocalDates(
            date,
            date,
            "America/New_York");

        Assert.Equal(
            new DateTime(2026, 3, 8, 5, 0, 0, DateTimeKind.Utc),
            result.StartUtc);

        Assert.Equal(
            new DateTime(2026, 3, 9, 4, 0, 0, DateTimeKind.Utc),
            result.EndUtc);

        Assert.Equal(TimeSpan.FromHours(23), result.EndUtc - result.StartUtc);
    }

    [Fact]
    public void FromInclusiveLocalDates_RejectsReversedRange()
    {
        DateOnly startDate = new(2026, 7, 31);
        DateOnly endDate = new(2026, 7, 30);

        Assert.Throws<ArgumentException>(() =>
            TimeZoneRange.FromInclusiveLocalDates(
                startDate,
                endDate,
                "America/Guayaquil"));
    }

    [Fact]
    public void FromInclusiveLocalDates_RejectsUnknownTimeZone()
    {
        DateOnly date = new(2026, 7, 30);

        Assert.Throws<ArgumentException>(() =>
            TimeZoneRange.FromInclusiveLocalDates(
                date,
                date,
                "Invalid/TimeZone"));
    }
}
