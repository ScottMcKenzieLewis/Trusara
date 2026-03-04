namespace Trusara.Domain.Process;

public enum ExtruderState
{
    Unknown = 0,
    Off,
    Warmup,
    Ready,
    Running,
    Alarmed,
    EStop
}