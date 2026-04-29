using System.Collections.Generic;
using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class PlayerTickProcessor
{
    private readonly PlayerTickContextProvider _playerTickContextProvider = new();
    private readonly PlayerMovementObserver _playerMovementObserver = new();
    private readonly PlayerTeleportProcessor _playerTeleportProcessor = new();
    private readonly PlayerEnforcementProcessor _playerEnforcementProcessor = new();
    private readonly PlayerThrottleProcessor _playerThrottleProcessor = new();

    public bool Process(IPlayer player, Dictionary<string, PlayerThrottleState> states, ChunkQueueTweaksConfig config, GlobalPressureState globalPressure, double tickSeconds)
    {
        var context = _playerTickContextProvider.Get(player, states, config, tickSeconds);
        if (context is null)
        {
            return false;
        }

        var observation = _playerMovementObserver.Observe(context.Value.Player, context.Value.State, config, tickSeconds);
        if (!observation.ShouldContinue)
        {
            return false;
        }

        if (!_playerTeleportProcessor.ShouldContinue(context.Value.Player, context.Value.State, observation.Sample, config))
        {
            return false;
        }

        var enforcement = _playerEnforcementProcessor.Process(context.Value.Player, context.Value.State, observation.Sample, config, tickSeconds);
        return enforcement.ShouldContinue
            && _playerThrottleProcessor.Process(context.Value.Player, context.Value.State, observation.Sample, config, globalPressure, tickSeconds);
    }
}
