namespace ChunkQueueTweaks;

internal sealed class SafePositionWriter
{
    public void Write(PlayerThrottleState state, MovementSample sample)
    {
        var safePosition = state.SafePosition;
        safePosition.HasSafePosition = true;
        safePosition.LastSafeX = sample.X;
        safePosition.LastSafeY = sample.Y;
        safePosition.LastSafeZ = sample.Z;
        safePosition.LastSafeInternalY = sample.InternalY;
        safePosition.LastSafeDimensionKey = sample.DimensionKey;
        safePosition.LastSafeChunkX = sample.ChunkX;
        safePosition.LastSafeChunkZ = sample.ChunkZ;
    }
}
