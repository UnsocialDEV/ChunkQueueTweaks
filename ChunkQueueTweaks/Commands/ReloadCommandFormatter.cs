namespace ChunkQueueTweaks;

internal sealed class ReloadCommandFormatter
{
    public string Format(bool reloaded)
    {
        return reloaded ? "ChunkQueueTweaks config reloaded." : "ChunkQueueTweaks config reload failed: server API unavailable.";
    }
}
