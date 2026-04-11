namespace Trusara.Plc.Abstractions.Tags;

public sealed record PlcValue<T>(
    T? Value,
    PlcValueQuality Quality,
    DateTimeOffset TimestampUtc);
