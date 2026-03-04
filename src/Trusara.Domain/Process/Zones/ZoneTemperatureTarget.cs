using Trusara.Domain.Units;

namespace Trusara.Domain.Process.Zones;

public readonly record struct ZoneTemperatureTarget(ZoneId Zone, TemperatureC Target)
{
    public override string ToString() => $"{Zone}: {Target}";
}