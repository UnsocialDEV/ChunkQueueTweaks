using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerMovementObserver
{
    private readonly MovementSampler _movementSampler = new();
    private readonly MovementHistoryUpdater _movementHistoryUpdater = new();
    private readonly SafePositionUpdater _safePositionUpdater = new();

    public MovementObservation Observe(IServerPlayer player, PlayerThrottleState state, ChunkQueueTweaksConfig config, double tickSeconds)
    {
        var sample = _movementSampler.Sample(player, state, tickSeconds);
        if (sample.HasPreviousPosition)
        {
            return new MovementObservation(sample, true);
        }

        _movementHistoryUpdater.Commit(state, sample);
        _safePositionUpdater.Update(state, sample, config);
        return new MovementObservation(sample, false);
    }
}
