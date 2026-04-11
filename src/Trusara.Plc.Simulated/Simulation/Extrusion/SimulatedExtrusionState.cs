namespace Trusara.Plc.Simulated.Simulation.Extrusion;

public sealed class SimulatedExtrusionState
{
    public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool IsConnected { get; set; } = true;
    public bool IsRunning { get; set; }

    public double ScrewSpeedRpm { get; set; }
    public double ScrewSpeedRpmTarget { get; set; }

    public double LineSpeedMpm { get; set; }
    public double LineSpeedMpmTarget { get; set; }

    public double MeltPressureBar { get; set; }

    public List<double> ZoneTemperaturesC { get; } = new();
    public List<double> ZoneTargetTemperaturesC { get; } = new();

    public void SetZoneTargets(IReadOnlyList<double> targets)
    {
        if (targets.Count != ZoneTargetTemperaturesC.Count)
            throw new InvalidOperationException("Zone target count mismatch.");

        for (var i = 0; i < targets.Count; i++)
        {
            ZoneTargetTemperaturesC[i] = targets[i];
        }
    }
}