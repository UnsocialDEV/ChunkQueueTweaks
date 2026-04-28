using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TeleportSignalDetector
{
    public TeleportSignal Detect(IServerPlayer player, MovementSample sample)
    {
        if (player.Entity.Teleporting || player.Entity.IsTeleport)
        {
            return new TeleportSignal(true, "engine teleport signal");
        }

        if (sample.DimensionChanged)
        {
            return new TeleportSignal(true, "dimension change");
        }

        return new TeleportSignal(false, string.Empty);
    }
}
