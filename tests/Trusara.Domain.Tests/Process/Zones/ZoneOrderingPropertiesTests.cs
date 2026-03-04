using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Trusara.Domain.Diagnostics;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Tests.Builders;

namespace Trusara.Domain.Tests.PropertyTesting;

public sealed class ZoneOrderingPropertiesTests
{
    private readonly ExtrusionRiskAssessor _sut = new();

    [Property(MaxTest = 150)]
    public void ReorderingZones_DoesNotChangeFlags(PositiveInt zoneCountRaw, int tempRaw, NonNegativeInt bumpRaw)
    {
        var zoneCount = Math.Clamp(zoneCountRaw.Get, 1, 12);
        var targetC = Math.Clamp(tempRaw, 120, 260);

        var recipe = ExtrusionTestBuilders.DefaultRecipe(zoneCount: zoneCount, zoneTargetC: targetC);
        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(recipe);

        // Nudge one zone beyond tolerance sometimes
        var tol = recipe.Tolerances.ZoneTempDeltaC;
        var bump = Math.Min(bumpRaw.Get / 10.0, 10.0);
        var offenderZone = ZoneId.Create((zoneCount % 2) + 1);
        readings = readings.WithZoneActual(offenderZone, targetC + tol + bump);

        // Reorder targets and actuals (reverse)
        var recipe2 = recipe with
        {
            Targets = recipe.Targets with { ZoneTemperatures = recipe.Targets.ZoneTemperatures.Reverse().ToList() }
        };

        var readings2 = readings with
        {
            ZoneTemperaturesActual = readings.ZoneTemperaturesActual.Reverse().ToList()
        };

        var r1 = _sut.Assess(recipe, readings);
        var r2 = _sut.Assess(recipe2, readings2);

        r1.Flags.Should().BeEquivalentTo(r2.Flags);
    }
}