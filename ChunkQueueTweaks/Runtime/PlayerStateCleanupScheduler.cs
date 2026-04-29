using System.Collections.Generic;
using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class PlayerStateCleanupScheduler
{
    private const double CleanupIntervalSeconds = 5.0;

    private readonly PlayerStateCleaner _playerStateCleaner = new();
    private double _cleanupElapsedSeconds = CleanupIntervalSeconds;

    public void Clean(Dictionary<string, PlayerThrottleState> states, IPlayer[] players, double tickSeconds)
    {
        _cleanupElapsedSeconds += tickSeconds;
        if (_cleanupElapsedSeconds < CleanupIntervalSeconds)
        {
            return;
        }

        _playerStateCleaner.Clean(states, players);
        _cleanupElapsedSeconds = 0.0;
    }
}
