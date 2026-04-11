using Trusara.Plc.Abstractions.Results;

namespace Trusara.Plc.Abstractions.Tags;

public interface IPlcTagWriter
{
    ValueTask<Result> WriteBoolAsync(string address, bool value, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteInt32Async(string address, int value, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteDoubleAsync(string address, double value, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteStringAsync(string address, string value, CancellationToken cancellationToken = default);
}
