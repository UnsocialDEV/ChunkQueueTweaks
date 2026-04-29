namespace ChunkQueueTweaks;

internal sealed class TeleportGraceClock
{
    public void Tick(PlayerThrottleState state, double dt)
    {
        var elapsedMs = (int)(dt * 1000.0);
        var teleport = state.Teleport;
        teleport.TeleportGraceMs = System.Math.Max(0, teleport.TeleportGraceMs - elapsedMs);
        teleport.TeleportHeuristicCooldownMs = System.Math.Max(0, teleport.TeleportHeuristicCooldownMs - elapsedMs);
    }
}
