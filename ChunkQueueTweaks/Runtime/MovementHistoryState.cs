namespace ChunkQueueTweaks;

internal sealed class MovementHistoryState
{
    public bool HasPreviousPosition;
    public double PreviousX;
    public double PreviousZ;
    public double PreviousInternalY;
    public int PreviousDimensionKey;
    public double PreviousDirectionX;
    public double PreviousDirectionZ;
}
