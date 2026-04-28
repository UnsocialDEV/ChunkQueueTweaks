namespace ChunkQueueTweaks;

internal readonly record struct PressureResult(
    bool IsOverSafeSpeed,
    bool IsSustainedLinearTravel,
    bool IsUnderPressure,
    double Severity);
