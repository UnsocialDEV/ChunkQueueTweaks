namespace ChunkQueueTweaks;

internal sealed class SafePositionUpdater
{
    private readonly SafePositionWriter _safePositionWriter = new();

    public void Update(PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config)
    {
        if (state.Correction.CorrectionCooldownMs > 0 || sample.Speed > config.MaxSafeHorizontalSpeed)
        {
            return;
        }

        _safePositionWriter.Write(state, sample);
    }
}
