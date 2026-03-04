using FluentAssertions;
using Trusara.Domain.Diagnostics;
using Trusara.Domain.Process;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Tests.Builders;
using Xunit;

namespace Trusara.Domain.Tests.Diagnostics;

public sealed class ExtrusionRiskAssessorTests
{
    private readonly ExtrusionRiskAssessor _sut = new();

    [Fact]
    public void Assess_WhenEverythingWithinTolerance_ShouldReturnNoFlags()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe(zoneCount: 4);
        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(recipe);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should().BeEmpty();

        result.ZoneDeviations
            .Should()
            .OnlyContain(d => Math.Abs(d.DeltaC) < 0.0001);
    }

    [Fact]
    public void Assess_WhenStateIsAlarmed_ShouldContainAlarmFlag()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe();

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(
            recipe,
            state: ExtruderState.Alarmed);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should()
            .Contain(ExtrusionRiskFlag.AlarmActive);
    }

    [Fact]
    public void Assess_WhenScrewRpmExceedsTolerance_ShouldFlagDeviation()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe();

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(
            recipe,
            screwRpmActual: recipe.Targets.ScrewSpeed.Value
                + recipe.Tolerances.ScrewSpeedRpmDelta + 0.01);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should()
            .Contain(ExtrusionRiskFlag.ScrewRpmDeviationHigh);
    }

    [Fact]
    public void Assess_WhenLineSpeedExceedsTolerance_ShouldFlagDeviation()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe();

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(
            recipe,
            lineMpmActual: recipe.Targets.LineSpeed.Value
                + recipe.Tolerances.LineSpeedMpmDelta + 0.01);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should()
            .Contain(ExtrusionRiskFlag.LineSpeedDeviationHigh);
    }

    [Fact]
    public void Assess_WhenZoneTemperatureExceedsTolerance_ShouldFlagAndRecordDeviation()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe(
            zoneCount: 5,
            zoneTargetC: 180);

        var readings = ExtrusionTestBuilders
            .ReadingsFromRecipe(recipe)
            .WithZoneActual(
                ZoneId.Create(3),
                180 + recipe.Tolerances.ZoneTempDeltaC + 0.5);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should()
            .Contain(ExtrusionRiskFlag.ZoneTemperatureDeviationHigh);

        result.ZoneDeviations.Should()
            .Contain(d =>
                d.Zone == ZoneId.Create(3) &&
                d.DeltaC > recipe.Tolerances.ZoneTempDeltaC);
    }

    [Fact]
    public void Assess_WhenMeltPressureTooHigh_ShouldFlagPressureRisk()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe(
            meltPressureHighLimitBar: 150);

        var tooHigh =
            recipe.Targets.MeltPressureHighLimit.Value
            + recipe.Tolerances.MeltPressureDelta.Value
            + 0.01;

        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(
            recipe,
            meltPressureBar: tooHigh);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should()
            .Contain(ExtrusionRiskFlag.MeltPressureHigh);
    }

    [Fact]
    public void Assess_WhenMultipleZonesOutOfTolerance_ShouldEmitSingleFlagType()
    {
        var recipe = ExtrusionTestBuilders.DefaultRecipe(
            zoneCount: 3,
            zoneTargetC: 180);

        var readings = ExtrusionTestBuilders
            .ReadingsFromRecipe(recipe)
            .WithZoneActual(ZoneId.Create(1),
                180 + recipe.Tolerances.ZoneTempDeltaC + 1)
            .WithZoneActual(ZoneId.Create(2),
                180 + recipe.Tolerances.ZoneTempDeltaC + 2);

        var result = _sut.Assess(recipe, readings);

        result.Flags
            .Should()
            .ContainSingle(f =>
                f == ExtrusionRiskFlag.ZoneTemperatureDeviationHigh);
    }
}