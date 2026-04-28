using System.Collections.Generic;
using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class PlayerStateCleaner
{
    private readonly List<string> _stalePlayerIds = new();

    public void Clean(Dictionary<string, PlayerThrottleState> states, IPlayer[] onlinePlayers)
    {
        if (states.Count == 0)
        {
            return;
        }

        _stalePlayerIds.Clear();
        foreach (var playerId in states.Keys)
        {
            var isOnline = false;
            foreach (var player in onlinePlayers)
            {
                if (player.PlayerUID == playerId)
                {
                    isOnline = true;
                    break;
                }
            }

            if (!isOnline)
            {
                _stalePlayerIds.Add(playerId);
            }
        }

        foreach (var playerId in _stalePlayerIds)
        {
            states.Remove(playerId);
        }
    }
}
