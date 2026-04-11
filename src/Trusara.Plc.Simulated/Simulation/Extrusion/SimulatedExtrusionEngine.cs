namespace Trusara.Plc.Simulated.Simulation.Extrusion;

public sealed class SimulatedExtrusionEngine
{
    private readonly SimulatedExtrusionState _state;

    public SimulatedExtrusionEngine(SimulatedExtrusionState state)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public void Tick(TimeSpan delta)
    {
        // advance temperatures toward targets
        // ramp line speed / screw speed
        // update derived pressure
        // update timestamp
    }
}