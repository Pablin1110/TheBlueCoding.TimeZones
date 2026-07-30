namespace TheBlueCoding.TimeZones;

public readonly record struct UtcDateRange(
    DateTime StartUtc,
    DateTime EndUtc);
