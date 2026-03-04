using Trusara.Domain.Process.Zones;
using Trusara.Domain.Units;

public sealed record ExtrusionTargets(
    Rpm ScrewSpeed,
    SpeedMpm LineSpeed,
    IReadOnlyList<ZoneTemperatureTarget> ZoneTemperatures,
    TemperatureC DieTemp,
    PressureBar MeltPressureHighLimit)
{
    public bool TryGetZoneTarget(ZoneId zone, out TemperatureC target)
    {
        foreach (var z in ZoneTemperatures)
        {
            if (z.Zone.Equals(zone))
            {
                target = z.Target;
                return true;
            }
        }

        target = default;
        return false;
    }
}
