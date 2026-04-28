namespace ChunkQueueTweaks;

internal sealed class SafePositionUpdater
{
    public void Update(PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config)
    {
        if (state.CorrectionCooldownMs > 0 || sample.Speed > config.MaxSafeHorizontalSpeed)
        {
            return;
        }

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
