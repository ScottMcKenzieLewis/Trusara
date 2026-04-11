using Trusara.Plc.Abstractions.Extrusion;
using Trusara.Plc.Abstractions.Results;
using Trusara.Plc.Simulated.Simulation.Extrusion;

namespace Trusara.Plc.Simulated.Gateway;

public sealed class SimulatedExtrusionPlcGateway : IExtrusionPlcGateway
{
    private readonly SimulatedExtrusionState _state;
    private readonly SimulatedExtrusionEngine _engine;

    public SimulatedExtrusionPlcGateway(
        SimulatedExtrusionState state,
        SimulatedExtrusionEngine engine)
    {
        _state = state;
        _engine = engine;
    }

    public ValueTask<ExtrusionPlcSnapshot> ReadSnapshotAsync(
        CancellationToken cancellationToken = default)
    {
        var snapshot = new ExtrusionPlcSnapshot(
            TimestampUtc: _state.TimestampUtc,
            IsConnected: _state.IsConnected,
            IsRunning: _state.IsRunning,
            ScrewSpeedRpm: _state.ScrewSpeedRpm,
            LineSpeedMpm: _state.LineSpeedMpm,
            MeltPressureBar: _state.MeltPressureBar,
            ZoneTemperaturesC: _state.ZoneTemperaturesC.ToArray(),
            ZoneTargetTemperaturesC: _state.ZoneTargetTemperaturesC.ToArray());

        return ValueTask.FromResult(snapshot);
    }

    public ValueTask<Result> ApplyCommandsAsync(
        ExtrusionCommandSet commands,
        CancellationToken cancellationToken = default)
    {
        if (!_state.IsConnected)
        {
            return ValueTask.FromResult(Result.Failure("PLC is not connected."));
        }

        if (commands.Start == true)
            _state.IsRunning = true;

        if (commands.Stop == true)
            _state.IsRunning = false;

        if (commands.ScrewSpeedRpm.HasValue)
            _state.ScrewSpeedRpmTarget = commands.ScrewSpeedRpm.Value;

        if (commands.LineSpeedMpm.HasValue)
            _state.LineSpeedMpmTarget = commands.LineSpeedMpm.Value;

        if (commands.ZoneTargetTemperaturesC is not null)
        {
            _state.SetZoneTargets(commands.ZoneTargetTemperaturesC);
        }

        return ValueTask.FromResult(Result.Success());
    }

    public void Tick(TimeSpan delta) => _engine.Tick(delta);
}
