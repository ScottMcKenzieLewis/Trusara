namespace Trusara.Domain.Units;

public readonly record struct SpeedMpm(double Value)
{
    public override string ToString() => $"{Value:0.##} m/min";

    public static SpeedMpm Clamp(SpeedMpm value, SpeedMpm min, SpeedMpm max)
        => new(Math.Clamp(value.Value, min.Value, max.Value));
}