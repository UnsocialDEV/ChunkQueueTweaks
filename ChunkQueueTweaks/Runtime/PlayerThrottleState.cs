namespace ChunkQueueTweaks;

internal sealed class PlayerThrottleState
{
    public MovementHistoryState MovementHistory { get; } = new();
    public SafePositionState SafePosition { get; } = new();
    public PressureTrackingState Pressure { get; } = new();
    public CorrectionTrackingState Correction { get; } = new();
    public TeleportTrackingState Teleport { get; } = new();
    public ThrottleTrackingState Throttle { get; } = new();
    public ObservationTrackingState Observation { get; } = new();
}
