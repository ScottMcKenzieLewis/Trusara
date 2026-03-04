using Trusara.Domain.Process.Zones;
using Trusara.Domain.Units;

namespace Trusara.Domain.Recipes;

public sealed record ExtrusionRecipe(
    string Name,
    string Material,
    ExtrusionTargets Targets,
    ExtrusionTolerances Tolerances)
{
    public override string ToString() => $"{Name} ({Material})";
}