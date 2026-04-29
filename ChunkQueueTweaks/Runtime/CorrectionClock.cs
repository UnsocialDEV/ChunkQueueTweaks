namespace ChunkQueueTweaks;

internal sealed class CorrectionClock
{
    public void Tick(PlayerThrottleState state, double dt)
    {
        var correction = state.Correction;
        correction.CorrectionCooldownMs = correction.CorrectionCooldownMs <= 0
            ? 0
            : int.Max(0, correction.CorrectionCooldownMs - (int)(dt * 1000.0));
    }
}
