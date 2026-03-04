using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Trusara.Domain.Process.Zones;
using Trusara.Domain.Units;

namespace Trusara.Domain.Tests.PropertyTesting;

public sealed class ZoneDeviationMathPropertiesTests
{
    [Property(MaxTest = 300)]
    public void ZoneDeviation_EqualsAbsoluteDifference(PositiveInt zoneRaw, int targetRaw, int actualRaw)
    {
        var zone = ZoneId.Create(Math.Clamp(zoneRaw.Get, 1, 50));
        var targetC = Math.Clamp(targetRaw, 0, 400);
        var actualC = Math.Clamp(actualRaw, 0, 400);

        var target = new ZoneTemperatureTarget(zone, new TemperatureC(targetC));
        var actual = new ZoneTemperatureReading(zone, new TemperatureC(actualC));

        var dev = ZoneDeviation.Between(target, actual);

        dev.DeltaC.Should().BeApproximately(Math.Abs(actualC - targetC), 1e-9);
        dev.DeltaC.Should().BeGreaterThanOrEqualTo(0);
    }
}