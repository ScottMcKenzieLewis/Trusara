namespace Trusara.Domain.Units;

public readonly record struct TemperatureC(double Value)
{
    public override string ToString() => $"{Value:0.##} °C";

    public static TemperatureC Clamp(TemperatureC value, TemperatureC min, TemperatureC max)
        => new(Math.Clamp(value.Value, min.Value, max.Value));
}