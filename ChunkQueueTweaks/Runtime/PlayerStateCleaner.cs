using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class PlayerStateCleaner
{
    public void Clean(Dictionary<string, PlayerThrottleState> states, IPlayer[] onlinePlayers)
    {
        if (states.Count == 0)
        {
            return;
        }

        var onlineIds = onlinePlayers.Select(player => player.PlayerUID).ToHashSet();
        foreach (var playerId in states.Keys.Where(playerId => !onlineIds.Contains(playerId)).ToArray())
        {
            states.Remove(playerId);
        }
    }
}
