using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Trusara.Domain.Diagnostics;
using Trusara.Domain.Tests.Builders;

namespace Trusara.Domain.Tests.PropertyTesting;

public sealed class PerfectMatchPropertiesTests
{
    private readonly ExtrusionRiskAssessor _sut = new();

    [Property(MaxTest = 200)]
    public void PerfectMatch_ShouldProduceNoFlags(PositiveInt zoneCountRaw, int tempRaw)
    {
        var zoneCount = Math.Clamp(zoneCountRaw.Get, 1, 20);
        var targetC = Math.Clamp(tempRaw, 120, 260);

        var recipe = ExtrusionTestBuilders.DefaultRecipe(zoneCount: zoneCount, zoneTargetC: targetC);

        // Readings builder matches recipe targets; pressure is recipe limit - 10 in builder.
        var readings = ExtrusionTestBuilders.ReadingsFromRecipe(recipe);

        var result = _sut.Assess(recipe, readings);

        result.Flags.Should().BeEmpty();
    }
}