using Trusara.Domain.Process.Zones;

namespace Trusara.Domain.Diagnostics;

public sealed record ExtrusionRiskAssessment(
    IReadOnlyList<ExtrusionRiskFlag> Flags,
    IReadOnlyList<ZoneDeviation> ZoneDeviations)
{
    public bool HasRisks => Flags.Count > 0;
}
