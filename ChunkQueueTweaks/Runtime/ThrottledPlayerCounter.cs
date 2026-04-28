using System.Collections.Generic;
using System.Linq;

namespace ChunkQueueTweaks;

internal sealed class ThrottledPlayerCounter
{
    public int Count(IEnumerable<PlayerThrottleState> states)
    {
        return states.Count(state => state.LastThrottleFactor < 0.999);
    }
}
