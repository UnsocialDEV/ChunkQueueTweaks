using System;
using System.Collections.Generic;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TickProcessor
{
    private readonly PlayerStateCleanupScheduler _playerStateCleanupScheduler = new();
    private readonly PlayerTickProcessor _playerTickProcessor = new();
    private readonly GlobalPressureEvaluator _globalPressureEvaluator = new();

    public GlobalPressureState Process(ICoreServerAPI api, Dictionary<string, PlayerThrottleState> states, ChunkQueueTweaksConfig config, GlobalPressureState globalPressure, float dt)
    {
        var players = api.World.AllOnlinePlayers;
        int pressuredPlayers = 0;
        var tickSeconds = Math.Max(dt, config.TickIntervalMs / 1000f);

        _playerStateCleanupScheduler.Clean(states, players, tickSeconds);

        foreach (var player in players)
        {
            if (_playerTickProcessor.Process(player, states, config, globalPressure, tickSeconds))
            {
                pressuredPlayers++;
            }
        }

        return new GlobalPressureState(_globalPressureEvaluator.Evaluate(pressuredPlayers, config));
    }
}
