namespace ChunkQueueTweaks;

internal sealed class SafePositionState
{
    public bool HasSafePosition;
    public double LastSafeX;
    public double LastSafeY;
    public double LastSafeZ;
    public double LastSafeInternalY;
    public int LastSafeDimensionKey;
    public int LastSafeChunkX;
    public int LastSafeChunkZ;
}
