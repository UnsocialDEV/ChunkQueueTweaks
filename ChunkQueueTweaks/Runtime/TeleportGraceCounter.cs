using System.Collections.Generic;
using System.Linq;

namespace ChunkQueueTweaks;

internal sealed class TeleportGraceCounter
{
    public int Count(IEnumerable<PlayerThrottleState> states)
    {
        return states.Count(state => state.TeleportGraceMs > 0);
    }
}
