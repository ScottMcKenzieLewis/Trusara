namespace Trusara.Domain.Units;

public readonly record struct Rpm(double Value)
{
    public override string ToString() => $"{Value:0.##} rpm";

    public static Rpm Clamp(Rpm value, Rpm min, Rpm max)
        => new(Math.Clamp(value.Value, min.Value, max.Value));
}