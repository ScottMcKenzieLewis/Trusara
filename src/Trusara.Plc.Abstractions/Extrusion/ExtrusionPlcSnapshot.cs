namespace Trusara.Plc.Abstractions.Extrusion;

public sealed record ExtrusionPlcSnapshot(
    DateTimeOffset TimestampUtc,
    bool IsConnected,
    bool IsRunning,
    double ScrewSpeedRpm,
    double LineSpeedMpm,
    double MeltPressureBar,
    IReadOnlyList<double> ZoneTemperaturesC,
    IReadOnlyList<double> ZoneTargetTemperaturesC);