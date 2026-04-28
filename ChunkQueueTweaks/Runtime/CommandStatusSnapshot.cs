namespace ChunkQueueTweaks;

internal readonly record struct CommandStatusSnapshot(
    bool Enabled,
    int TrackedPlayers,
    bool GlobalPressureActive,
    int ThrottledPlayers,
    int CorrectedPlayers,
    double HardMaxHorizontalSpeed,
    int MaxChunkColumnsPerTick,
    int TeleportGracePlayers,
    bool TeleportHeuristicEnabled);
