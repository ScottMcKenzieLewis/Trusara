using Trusara.Plc.Abstractions.Results;

namespace Trusara.Plc.Abstractions.Extrusion;

public interface IExtrusionPlcGateway
{
    ValueTask<ExtrusionPlcSnapshot> ReadSnapshotAsync(CancellationToken cancellationToken = default);
    ValueTask<Result> ApplyCommandsAsync(ExtrusionCommandSet commands, CancellationToken cancellationToken = default);
}