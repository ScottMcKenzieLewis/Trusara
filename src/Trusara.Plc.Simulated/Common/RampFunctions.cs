namespace Trusara.Plc.Simulated.Common;

public static class RampFunctions
{
    public static double MoveToward(
        double current,
        double target,
        double maxDelta)
    {
        if (maxDelta < 0)
            throw new ArgumentOutOfRangeException(nameof(maxDelta), "maxDelta must be non-negative.");

        var difference = target - current;

        if (Math.Abs(difference) <= maxDelta)
            return target;

        return current + Math.Sign(difference) * maxDelta;
    }

    public static double RampPerSecond(
        double current,
        double target,
        double ratePerSecond,
        TimeSpan delta)
    {
        if (ratePerSecond < 0)
            throw new ArgumentOutOfRangeException(nameof(ratePerSecond), "ratePerSecond must be non-negative.");

        if (delta < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(delta), "delta must be non-negative.");

        var maxDelta = ratePerSecond * delta.TotalSeconds;
        return MoveToward(current, target, maxDelta);
    }

    public static IReadOnlyList<double> RampPerSecond(
        IReadOnlyList<double> current,
        IReadOnlyList<double> target,
        double ratePerSecond,
        TimeSpan delta)
    {
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(target);

        if (current.Count != target.Count)
            throw new ArgumentException("Current and target collections must have the same number of elements.");

        var results = new double[current.Count];

        for (var i = 0; i < current.Count; i++)
        {
            results[i] = RampPerSecond(current[i], target[i], ratePerSecond, delta);
        }

        return results;
    }
}
