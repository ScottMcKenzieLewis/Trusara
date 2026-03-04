namespace Trusara.Domain.Units;

public readonly record struct PressureBar(double Value)
{
    public override string ToString() => $"{Value:0.##} bar";

    public static PressureBar Clamp(PressureBar value, PressureBar min, PressureBar max)
        => new(Math.Clamp(value.Value, min.Value, max.Value));
}