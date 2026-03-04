using Trusara.Domain.Process.Zones;
using Trusara.Domain.Units;

namespace Trusara.Domain.Process;

/// <summary>
/// Operator / recipe targets that may be written to the PLC.
/// Domain stays in canonical units (e.g., °C).
/// </summary>
public sealed record ExtrusionSetpoints(
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