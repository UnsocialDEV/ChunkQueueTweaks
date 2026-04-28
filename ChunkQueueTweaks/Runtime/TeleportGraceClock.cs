namespace ChunkQueueTweaks;

internal sealed class TeleportGraceClock
{
    public void Tick(PlayerThrottleState state, double dt)
    {
        var elapsedMs = (int)(dt * 1000.0);
        state.TeleportGraceMs = System.Math.Max(0, state.TeleportGraceMs - elapsedMs);
        state.TeleportHeuristicCooldownMs = System.Math.Max(0, state.TeleportHeuristicCooldownMs - elapsedMs);
    }
}
