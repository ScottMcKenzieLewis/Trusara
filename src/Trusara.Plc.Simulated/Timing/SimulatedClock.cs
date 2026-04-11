namespace Trusara.Plc.Simulated.Timing;

public sealed class SimulatedClock
{
    public SimulatedClock(DateTimeOffset initialUtc)
    {
        if (initialUtc.Offset != TimeSpan.Zero)
            initialUtc = initialUtc.ToUniversalTime();

        CurrentUtc = initialUtc;
    }

    public DateTimeOffset CurrentUtc { get; private set; }

    public void Advance(TimeSpan delta)
    {
        if (delta < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(delta), "delta must be non-negative.");

        CurrentUtc = CurrentUtc.Add(delta);
    }

    public void Set(DateTimeOffset valueUtc)
    {
        if (valueUtc.Offset != TimeSpan.Zero)
            valueUtc = valueUtc.ToUniversalTime();

        CurrentUtc = valueUtc;
    }
}