using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Trusara.Domain.Diagnostics;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Tests.Builders;

namespace Trusara.Domain.Tests.PropertyTesting;

public sealed class ZoneDeviationPropertiesTests
{
    private readonly ExtrusionRiskAssessor _sut = new();

    [Property(MaxTest = 200)]
    public void IfAllZonesWithinTolerance_ThenNoZoneDeviationFlag(PositiveInt zoneCountRaw, int tempRaw, NonNegativeInt deltaRaw)
    {
        var zoneCount = Math.Clamp(zoneCountRaw.Get, 1, 20);
        var targetC = Math.Clamp(tempRaw, 120, 260);

        // Choose delta in [0, tol] (inclusive).
        var recipe = ExtrusionTestBuilders.DefaultRecipe(zoneCount: zoneCount, zoneTargetC: targetC);
        var tol = recipe.Tolerances.ZoneTempDeltaC;

        var delta = Math.Min(deltaRaw.Get / 10.0, tol); // <= tol

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(recipe);
        for (int i = 1; i <= zoneCount; i++)
            readings = readings.WithZoneActual(ZoneId.Create(i), targetC + delta);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should().NotContain(ExtrusionRiskFlag.ZoneTemperatureDeviationHigh);
    }

    [Property(MaxTest = 200)]
    public void IfAnyZoneExceedsTolerance_ThenZoneDeviationFlagIsPresent(PositiveInt zoneCountRaw, int tempRaw, PositiveInt offenderRaw, NonNegativeInt exceedRaw)
    {
        var zoneCount = Math.Clamp(zoneCountRaw.Get, 1, 20);
        var targetC = Math.Clamp(tempRaw, 120, 260);

        var recipe = ExtrusionTestBuilders.DefaultRecipe(zoneCount: zoneCount, zoneTargetC: targetC);
        var tol = recipe.Tolerances.ZoneTempDeltaC;

        // Choose an offending zone in [1..zoneCount]
        var offender = (offenderRaw.Get % zoneCount) + 1;

        // Choose exceed in (0, 10] and add to tolerance
        var exceed = Math.Max(0.1, Math.Min(exceedRaw.Get / 10.0, 10.0));
        var actualOffender = targetC + tol + exceed;

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(recipe)
            .WithZoneActual(ZoneId.Create(offender), actualOffender);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should().Contain(ExtrusionRiskFlag.ZoneTemperatureDeviationHigh);

        result.ZoneDeviations.Should().Contain(d =>
            d.Zone == ZoneId.Create(offender) &&
            d.DeltaC > tol);
    }
}