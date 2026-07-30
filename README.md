# TheBlueCoding.TimeZones

Shared timezone conversion utilities for TheBlueCoding .NET microservices.

The library interprets calendar dates in an IANA timezone and produces a
half-open UTC range suitable for querying PostgreSQL `timestamptz` columns.

## Compatibility

- .NET 6
- .NET 7
- .NET 8

## Usage

```csharp
using TheBlueCoding.TimeZones;

UtcDateRange range = TimeZoneRange.FromInclusiveLocalDates(
    new DateOnly(2026, 7, 30),
    new DateOnly(2026, 7, 30),
    "America/Guayaquil");
```

Use the resulting range in an Entity Framework query:

```csharp
var records = await context.Set<MedicalRecord>()
    .Where(record =>
        record.CreatedAt >= range.StartUtc &&
        record.CreatedAt < range.EndUtc)
    .ToListAsync();
```

`EndUtc` is exclusive. This avoids precision problems at the end of a day and
correctly handles daylight-saving transitions.

## Storage rule

Store instants such as `created_at` and `updated_at` as UTC in PostgreSQL
`timestamp with time zone` (`timestamptz`) columns. Use the timezone from the
authenticated request only to create UTC query boundaries or present values.
