using Trusara.Domain.Units;

namespace Trusara.Domain.Process.Zones;

public readonly record struct ZoneDeviation(ZoneId Zone, double DeltaC)
{
    public override string ToString() => $"{Zone}: Δ{DeltaC:0.##}°C";

    public static ZoneDeviation Between(ZoneTemperatureTarget target, ZoneTemperatureReading actual)
        => new(target.Zone, Math.Abs(actual.Actual.Value - target.Target.Value));
}