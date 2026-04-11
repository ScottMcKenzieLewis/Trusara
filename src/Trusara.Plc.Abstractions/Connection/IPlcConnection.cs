namespace Trusara.Plc.Abstractions.Connection;

public interface IPlcConnection : IAsyncDisposable
{
    ValueTask ConnectAsync(CancellationToken cancellationToken = default);
    ValueTask DisconnectAsync(CancellationToken cancellationToken = default);
    bool IsConnected { get; }
}