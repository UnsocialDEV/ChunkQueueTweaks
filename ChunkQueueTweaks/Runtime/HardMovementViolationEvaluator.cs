using System;

namespace ChunkQueueTweaks;

internal sealed class HardMovementViolationEvaluator
{
    public HardMovementViolation Evaluate(MovementSample sample, PlayerThrottleState state, ChunkQueueTweaksConfig config)
    {
        if (sample.Speed > config.HardMaxHorizontalSpeed)
        {
            return new HardMovementViolation(true, "hard speed limit exceeded");
        }

        if (state.HasSafePosition)
        {
            var chunkDeltaX = Math.Abs(sample.ChunkX - state.LastSafeChunkX);
            var chunkDeltaZ = Math.Abs(sample.ChunkZ - state.LastSafeChunkZ);
            if (Math.Max(chunkDeltaX, chunkDeltaZ) > config.MaxChunkColumnsPerTick)
            {
                return new HardMovementViolation(true, "chunk column jump exceeded");
            }
        }

        if (sample.Speed > config.MaxSafeHorizontalSpeed && state.ExtremeViolationScore >= config.MaxExtremeViolationScore)
        {
            return new HardMovementViolation(true, "repeated high-speed movement");
        }

        return new HardMovementViolation(false, string.Empty);
    }
}
