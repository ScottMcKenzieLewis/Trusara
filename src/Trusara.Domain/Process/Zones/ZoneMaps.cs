using Trusara.Domain.Units;

namespace Trusara.Domain.Process.Zones;

public static class ZoneMaps
{
    public static IReadOnlyDictionary<ZoneId, TemperatureC> ToTargetMap(this IReadOnlyList<ZoneTemperatureTarget> zones)
    {
        var dict = new Dictionary<ZoneId, TemperatureC>(zones.Count);
        foreach (var z in zones) dict[z.Zone] = z.Target;
        return dict;
    }

    public static IReadOnlyDictionary<ZoneId, TemperatureC> ToActualMap(this IReadOnlyList<ZoneTemperatureReading> zones)
    {
        var dict = new Dictionary<ZoneId, TemperatureC>(zones.Count);
        foreach (var z in zones) dict[z.Zone] = z.Actual;
        return dict;
    }
}
