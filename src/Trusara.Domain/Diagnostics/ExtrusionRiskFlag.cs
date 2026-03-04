namespace Trusara.Domain.Diagnostics;

public enum ExtrusionRiskFlag
{
    // Thermal
    ZoneTemperatureDeviationHigh,
    MeltTemperatureOutOfRange,

    // Mechanical
    MotorCurrentHigh,
    ScrewRpmDeviationHigh,

    // Process
    MeltPressureHigh,
    MeltPressureLow,
    LineSpeedDeviationHigh,

    // Safety / control
    VacuumLow,
    AlarmActive
}