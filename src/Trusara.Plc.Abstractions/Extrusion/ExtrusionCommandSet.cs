namespace Trusara.Plc.Abstractions.Extrusion;

public sealed record ExtrusionCommandSet(
    bool? Start = null,
    bool? Stop = null,
    double? ScrewSpeedRpm = null,
    double? LineSpeedMpm = null,
    IReadOnlyList<double>? ZoneTargetTemperaturesC = null);