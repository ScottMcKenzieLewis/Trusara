using Trusara.Domain.Units;

public sealed record ExtrusionTolerances(
    double ScrewSpeedRpmDelta,
    double LineSpeedMpmDelta,
    double ZoneTempDeltaC,
    double DieTempDeltaC,
    PressureBar MeltPressureDelta);
