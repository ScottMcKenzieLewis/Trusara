using Trusara.Domain.Units;

namespace Trusara.Domain.Process.Zones;

public readonly record struct ZoneTemperatureReading(ZoneId Zone, TemperatureC Actual)
{
    public override string ToString() => $"{Zone}: {Actual}";
}