namespace Trusara.Domain.Process.Zones;

/// <summary>
/// Identifies a temperature control zone (e.g., barrel zones 1..N).
/// </summary>
public readonly record struct ZoneId(int Value)
{
    public override string ToString() => $"Z{Value}";

    public static ZoneId Create(int value)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "ZoneId must be >= 1.");
        return new ZoneId(value);
    }
}