namespace Trusara.Plc.Abstractions.Tags;

public interface IPlcTagReader
{
    ValueTask<Result<bool>> ReadBoolAsync(string address, CancellationToken cancellationToken = default);
    ValueTask<Result<int>> ReadInt32Async(string address, CancellationToken cancellationToken = default);
    ValueTask<Result<double>> ReadDoubleAsync(string address, CancellationToken cancellationToken = default);
    ValueTask<Result<string>> ReadStringAsync(string address, CancellationToken cancellationToken = default);
}
