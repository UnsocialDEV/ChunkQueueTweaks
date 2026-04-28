using System.Collections.Generic;
using System.Linq;

namespace ChunkQueueTweaks;

internal sealed class CorrectedPlayerCounter
{
    public int Count(IEnumerable<PlayerThrottleState> states)
    {
        return states.Count(state => state.CorrectedLastTick || state.CorrectionCooldownMs > 0);
    }
}
