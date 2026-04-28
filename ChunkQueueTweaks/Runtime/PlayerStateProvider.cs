using System.Collections.Generic;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerStateProvider
{
    public PlayerThrottleState Get(Dictionary<string, PlayerThrottleState> states, IServerPlayer player)
    {
        if (!states.TryGetValue(player.PlayerUID, out var state))
        {
            state = new PlayerThrottleState();
            states[player.PlayerUID] = state;
        }

        return state;
    }
}
