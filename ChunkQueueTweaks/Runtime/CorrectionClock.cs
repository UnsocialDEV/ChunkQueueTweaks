namespace ChunkQueueTweaks;

internal sealed class CorrectionClock
{
    public void Tick(PlayerThrottleState state, double dt)
    {
        state.CorrectionCooldownMs = state.CorrectionCooldownMs <= 0
            ? 0
            : int.Max(0, state.CorrectionCooldownMs - (int)(dt * 1000.0));
    }
}
