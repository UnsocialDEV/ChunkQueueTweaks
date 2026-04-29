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

        var safePosition = state.SafePosition;
        if (safePosition.HasSafePosition)
        {
            var chunkDeltaX = Math.Abs(sample.ChunkX - safePosition.LastSafeChunkX);
            var chunkDeltaZ = Math.Abs(sample.ChunkZ - safePosition.LastSafeChunkZ);
            if (Math.Max(chunkDeltaX, chunkDeltaZ) > config.MaxChunkColumnsPerTick)
            {
                return new HardMovementViolation(true, "chunk column jump exceeded");
            }
        }

        if (sample.Speed > config.MaxSafeHorizontalSpeed && state.Pressure.ExtremeViolationScore >= config.MaxExtremeViolationScore)
        {
            return new HardMovementViolation(true, "repeated high-speed movement");
        }

        return new HardMovementViolation(false, string.Empty);
    }
}
