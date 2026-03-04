using System.Runtime.Intrinsics.Arm;
using Trusara.Domain.Process;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Recipes;
using Trusara.Domain.Units;

namespace Trusara.Domain.Tests.Builders;

public static class ExtrusionTestBuilders
{
    public static ExtrusionRecipe DefaultRecipe(
        int zoneCount = 3,
        double zoneTargetC = 180,
        double dieTargetC = 190,
        double screwRpm = 45,
        double lineMpm = 2.5,
        double meltPressureHighLimitBar = 150)
    {
        var zones = Enumerable.Range(1, zoneCount)
            .Select(i => new ZoneTemperatureTarget(ZoneId.Create(i), new TemperatureC(zoneTargetC)))
            .ToList();

        return new ExtrusionRecipe(
            Name: "Default",
            Material: "PVC",
            Targets: new ExtrusionTargets(
                ScrewSpeed: new Rpm(screwRpm),
                LineSpeed: new SpeedMpm(lineMpm),
                ZoneTemperatures: zones,
                DieTemp: new TemperatureC(dieTargetC),
                MeltPressureHighLimit: new PressureBar(meltPressureHighLimitBar)),
            Tolerances: new ExtrusionTolerances(
                ScrewSpeedRpmDelta: 2.0,
                LineSpeedMpmDelta: 0.2,
                ZoneTempDeltaC: 3.0,
                DieTempDeltaC: 3.0,
                MeltPressureDelta: new PressureBar(5.0)
            )
        );
    }

    public static ExtrusionReadings ReadingsFromRecipe(
        ExtrusionRecipe recipe,
        DateTimeOffset? ts = null,
        ExtruderState state = ExtruderState.Running,
        double? screwRpmActual = null,
        double? lineMpmActual = null,
        double? dieTempActualC = null,
        double? meltPressureBar = null,
        double motorAmps = 50,
        double vacuumKpa = 0)
    {
        var zoneActual = recipe.Targets.ZoneTemperatures
            .Select(z => new ZoneTemperatureReading(z.Zone, z.Target))
            .ToList();

        return new ExtrusionReadings(
            Timestamp: ts ?? DateTimeOffset.UtcNow,
            State: state,
            ScrewSpeedActual: new Rpm(screwRpmActual ?? recipe.Targets.ScrewSpeed.Value),
            LineSpeedActual: new SpeedMpm(lineMpmActual ?? recipe.Targets.LineSpeed.Value),
            ZoneTemperaturesActual: zoneActual,
            DieTempActual: new TemperatureC(dieTempActualC ?? recipe.Targets.DieTemp.Value),
            MeltPressure: new PressureBar(meltPressureBar ?? (recipe.Targets.MeltPressureHighLimit.Value - 10)),
            MotorCurrentAmps: motorAmps,
            VacuumKpa: vacuumKpa
        );
    }

    public static ExtrusionReadings WithZoneActual(
        this ExtrusionReadings readings,
        ZoneId zone,
        double actualC)
    {
        var zones = readings.ZoneTemperaturesActual.ToList();

        var idx = zones.FindIndex(z => z.Zone.Equals(zone));
        if (idx < 0)
            zones.Add(new ZoneTemperatureReading(zone, new TemperatureC(actualC)));
        else
            zones[idx] = new ZoneTemperatureReading(zone, new TemperatureC(actualC));

        return readings with { ZoneTemperaturesActual = zones };
    }
}