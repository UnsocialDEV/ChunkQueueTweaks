namespace ChunkQueueTweaks;

internal sealed class TeleportTrackingState
{
    public int TeleportGraceMs;
    public int TeleportHeuristicCooldownMs;
    public double LastAcceptedTeleportTotalSeconds = -9999.0;
    public int AcceptedTeleportCount;
}
