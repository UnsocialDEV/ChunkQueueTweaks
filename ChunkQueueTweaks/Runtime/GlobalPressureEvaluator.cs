namespace ChunkQueueTweaks;

internal sealed class GlobalPressureEvaluator
{
    public bool Evaluate(int pressuredPlayers, ChunkQueueTweaksConfig config)
    {
        return pressuredPlayers >= config.GlobalPressurePlayerCount;
    }
}
