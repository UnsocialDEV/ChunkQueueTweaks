namespace ChunkQueueTweaks;

internal sealed class StatusCommandFormatter
{
    public string Format(CommandStatusSnapshot snapshot)
    {
        return $"ChunkQueueTweaks enabled={snapshot.Enabled}, trackedPlayers={snapshot.TrackedPlayers}, throttledPlayers={snapshot.ThrottledPlayers}, correctedPlayers={snapshot.CorrectedPlayers}, teleportGracePlayers={snapshot.TeleportGracePlayers}, teleportHeuristic={snapshot.TeleportHeuristicEnabled}, globalPressure={snapshot.GlobalPressureActive}, hardMaxSpeed={snapshot.HardMaxHorizontalSpeed:0.##}, maxChunkColumnsPerTick={snapshot.MaxChunkColumnsPerTick}";
    }
}
