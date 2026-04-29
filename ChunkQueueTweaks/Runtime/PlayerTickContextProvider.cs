using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerTickContextProvider
{
    private readonly PlayerEligibility _playerEligibility = new();
    private readonly PlayerStateProvider _playerStateProvider = new();
    private readonly CorrectionClock _correctionClock = new();
    private readonly TeleportGraceClock _teleportGraceClock = new();

    public PlayerTickContext? Get(IPlayer player, Dictionary<string, PlayerThrottleState> states, ChunkQueueTweaksConfig config, double tickSeconds)
    {
        if (player is not IServerPlayer serverPlayer || !_playerEligibility.IsEligible(serverPlayer, config))
        {
            return null;
        }

        var state = _playerStateProvider.Get(states, serverPlayer);
        state.Correction.CorrectedLastTick = false;
        state.Observation.ObservedTotalSeconds += tickSeconds;
        _correctionClock.Tick(state, tickSeconds);
        _teleportGraceClock.Tick(state, tickSeconds);
        return new PlayerTickContext(serverPlayer, state);
    }
}
