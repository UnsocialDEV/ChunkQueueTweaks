namespace ChunkQueueTweaks;

internal sealed class SafePositionWriter
{
    public void Write(PlayerThrottleState state, MovementSample sample)
    {
        state.HasSafePosition = true;
        state.LastSafeX = sample.X;
        state.LastSafeY = sample.Y;
        state.LastSafeZ = sample.Z;
        state.LastSafeInternalY = sample.InternalY;
        state.LastSafeDimensionKey = sample.DimensionKey;
        state.LastSafeChunkX = sample.ChunkX;
        state.LastSafeChunkZ = sample.ChunkZ;
    }
}
