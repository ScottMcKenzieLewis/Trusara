using Trusara.Domain.Process.Zones;
using Trusara.Domain.Units;

namespace Trusara.Domain.Process;

/// <summary>
/// Measurements read from PLC / sensors at a point in time.
/// </summary>
public sealed record ExtrusionReadings(
    DateTimeOffset Timestamp,
    ExtruderState State,
    Rpm ScrewSpeedActual,
    SpeedMpm LineSpeedActual,
    IReadOnlyList<ZoneTemperatureReading> ZoneTemperaturesActual,
    TemperatureC DieTempActual,
    PressureBar MeltPressure,
    double MotorCurrentAmps,
    double VacuumKpa)
{
    public bool TryGetZoneActual(ZoneId zone, out TemperatureC actual)
    {
        foreach (var z in ZoneTemperaturesActual)
        {
            if (z.Zone.Equals(zone))
            {
                actual = z.Actual;
                return true;
            }
        }

        actual = default;
        return false;
    }
}