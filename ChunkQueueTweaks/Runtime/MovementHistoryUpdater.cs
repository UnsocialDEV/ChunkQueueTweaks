namespace ChunkQueueTweaks;

internal sealed class MovementHistoryUpdater
{
    public void Commit(PlayerThrottleState state, MovementSample sample)
    {
        var history = state.MovementHistory;
        history.HasPreviousPosition = true;
        history.PreviousX = sample.X;
        history.PreviousZ = sample.Z;
        history.PreviousInternalY = sample.InternalY;
        history.PreviousDimensionKey = sample.DimensionKey;
        history.PreviousDirectionX = sample.DirectionX;
        history.PreviousDirectionZ = sample.DirectionZ;
    }
}
