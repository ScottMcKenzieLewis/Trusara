using Trusara.Domain.Process;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Recipes;

namespace Trusara.Domain.Diagnostics;

public sealed class ExtrusionRiskAssessor
{
    public ExtrusionRiskAssessment Assess(ExtrusionRecipe recipe, ExtrusionReadings readings)
    {
        ArgumentNullException.ThrowIfNull(recipe);
        ArgumentNullException.ThrowIfNull(readings);

        var flags = new List<ExtrusionRiskFlag>(capacity: 8);
        var deviations = new List<ZoneDeviation>();

        // If PLC is reporting alarmed state, surface it as a risk flag.
        if (readings.State is ExtruderState.Alarmed or ExtruderState.EStop)
        {
            flags.Add(ExtrusionRiskFlag.AlarmActive);
            // Continue checking—still useful to see what else is off.
        }

        // ----------------------------
        // RPM deviation
        // ----------------------------
        var rpmDelta = Math.Abs(readings.ScrewSpeedActual.Value - recipe.Targets.ScrewSpeed.Value);
        if (rpmDelta > recipe.Tolerances.ScrewSpeedRpmDelta)
            flags.Add(ExtrusionRiskFlag.ScrewRpmDeviationHigh);

        // ----------------------------
        // Line speed deviation
        // ----------------------------
        var lineDelta = Math.Abs(readings.LineSpeedActual.Value - recipe.Targets.LineSpeed.Value);
        if (lineDelta > recipe.Tolerances.LineSpeedMpmDelta)
            flags.Add(ExtrusionRiskFlag.LineSpeedDeviationHigh);

        // ----------------------------
        // Die temperature deviation
        // ----------------------------
        var dieDelta = Math.Abs(readings.DieTempActual.Value - recipe.Targets.DieTemp.Value);
        if (dieDelta > recipe.Tolerances.DieTempDeltaC)
            flags.Add(ExtrusionRiskFlag.MeltTemperatureOutOfRange);

        // ----------------------------
        // Melt pressure checks
        // ----------------------------
        if (readings.MeltPressure.Value > recipe.Targets.MeltPressureHighLimit.Value + recipe.Tolerances.MeltPressureDelta.Value)
            flags.Add(ExtrusionRiskFlag.MeltPressureHigh);

        // Optional: low pressure (useful if you add a low limit later)
        if (readings.MeltPressure.Value < recipe.Targets.MeltPressureHighLimit.Value - (2 * recipe.Tolerances.MeltPressureDelta.Value))
            flags.Add(ExtrusionRiskFlag.MeltPressureLow);

        // ----------------------------
        // Motor current (simple early threshold)
        // ----------------------------
        // This is intentionally simple. Later you can add a recipe-defined limit.
        if (readings.MotorCurrentAmps >= 0 && readings.MotorCurrentAmps > 0) { /* placeholder */ }
        if (readings.MotorCurrentAmps > 120) // TODO: make recipe/configurable
            flags.Add(ExtrusionRiskFlag.MotorCurrentHigh);

        // ----------------------------
        // Vacuum
        // ----------------------------
        // Many systems treat vacuum as a minimum/low alarm.
        if (readings.VacuumKpa < -10) // TODO: make recipe/configurable; placeholder
            flags.Add(ExtrusionRiskFlag.VacuumLow);

        // ----------------------------
        // Zone temperature deviations
        // ----------------------------
        // Compare zones that exist in BOTH recipe and readings.
        var actualMap = readings.ZoneTemperaturesActual.ToActualMap();

        foreach (var target in recipe.Targets.ZoneTemperatures)
        {
            if (!actualMap.TryGetValue(target.Zone, out var actual))
                continue;

            var dev = new ZoneDeviation(target.Zone, Math.Abs(actual.Value - target.Target.Value));
            deviations.Add(dev);

            if (dev.DeltaC > recipe.Tolerances.ZoneTempDeltaC)
                flags.Add(ExtrusionRiskFlag.ZoneTemperatureDeviationHigh);
        }

        // De-dupe flags (same flag could be added multiple times by zones)
        flags = flags.Distinct().ToList();

        return new ExtrusionRiskAssessment(flags, deviations);
    }
}