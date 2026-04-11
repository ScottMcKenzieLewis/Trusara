namespace Trusara.Plc.Simulated.Faults;

public sealed class SimulatedFaultInjector
{
    private readonly Random _random;

    public SimulatedFaultInjector()
        : this(new Random())
    {
    }

    public SimulatedFaultInjector(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public bool CommunicationFaultActive { get; private set; }

    public bool TemperatureSensorFaultActive { get; private set; }

    public bool PressureSensorFaultActive { get; private set; }

    public bool ForceCommunicationFault { get; set; }

    public bool ForceTemperatureSensorFault { get; set; }

    public bool ForcePressureSensorFault { get; set; }

    /// <summary>
    /// 0.0 to 1.0 chance per evaluation that a comms fault will begin.
    /// </summary>
    public double CommunicationFaultProbability { get; set; }

    /// <summary>
    /// 0.0 to 1.0 chance per evaluation that a temperature sensor fault will begin.
    /// </summary>
    public double TemperatureSensorFaultProbability { get; set; }

    /// <summary>
    /// 0.0 to 1.0 chance per evaluation that a pressure sensor fault will begin.
    /// </summary>
    public double PressureSensorFaultProbability { get; set; }

    public void EvaluateFaults()
    {
        CommunicationFaultActive =
            ForceCommunicationFault || ShouldTrigger(CommunicationFaultProbability);

        TemperatureSensorFaultActive =
            ForceTemperatureSensorFault || ShouldTrigger(TemperatureSensorFaultProbability);

        PressureSensorFaultActive =
            ForcePressureSensorFault || ShouldTrigger(PressureSensorFaultProbability);
    }

    public void ClearAllFaults()
    {
        CommunicationFaultActive = false;
        TemperatureSensorFaultActive = false;
        PressureSensorFaultActive = false;

        ForceCommunicationFault = false;
        ForceTemperatureSensorFault = false;
        ForcePressureSensorFault = false;
    }

    private bool ShouldTrigger(double probability)
    {
        if (probability < 0.0 || probability > 1.0)
            throw new InvalidOperationException("Fault probability must be between 0.0 and 1.0.");

        return _random.NextDouble() < probability;
    }
}