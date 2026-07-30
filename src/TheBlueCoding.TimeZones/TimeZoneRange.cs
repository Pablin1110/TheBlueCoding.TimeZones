namespace TheBlueCoding.TimeZones;

public static class TimeZoneRange
{
    public static UtcDateRange FromInclusiveLocalDates(
        DateOnly startDate,
        DateOnly endDate,
        string timeZoneId)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException(
                "The end date cannot be earlier than the start date.",
                nameof(endDate));
        }

        TimeZoneInfo timeZone = GetTimeZone(timeZoneId);

        DateTime startLocal = startDate.ToDateTime(
            TimeOnly.MinValue,
            DateTimeKind.Unspecified);

        DateTime endLocalExclusive = endDate
            .AddDays(1)
            .ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Unspecified);

        return new UtcDateRange(
            TimeZoneInfo.ConvertTimeToUtc(startLocal, timeZone),
            TimeZoneInfo.ConvertTimeToUtc(endLocalExclusive, timeZone));
    }

    private static TimeZoneInfo GetTimeZone(string timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            throw new ArgumentException(
                "A time zone identifier is required.",
                nameof(timeZoneId));
        }

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException exception)
        {
            throw new ArgumentException(
                $"The time zone '{timeZoneId}' was not found.",
                nameof(timeZoneId),
                exception);
        }
        catch (InvalidTimeZoneException exception)
        {
            throw new ArgumentException(
                $"The time zone '{timeZoneId}' is invalid.",
                nameof(timeZoneId),
                exception);
        }
    }
}
