using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class ChunkRadiusPressureReducer
{
    public void Reduce(IServerPlayer player, ChunkQueueTweaksConfig config)
    {
        if (config.ReduceChunkRadiusOnCorrection)
        {
            player.CurrentChunkSentRadius = config.CorrectiveChunkSentRadius;
        }
    }
}
