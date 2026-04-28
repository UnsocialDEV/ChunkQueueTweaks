using System;

namespace ChunkQueueTweaks;

internal sealed class ExtremeViolationScoreUpdater
{
    public double Update(double currentScore, MovementSample sample, HardMovementViolation violation, ChunkQueueTweaksConfig config, double dt)
    {
        if (violation.IsViolation)
        {
            return Math.Min(config.MaxExtremeViolationScore, currentScore + config.ExtremeViolationIncrease);
        }

        if (sample.Speed > config.MaxSafeHorizontalSpeed)
        {
            return Math.Min(config.MaxExtremeViolationScore, currentScore + config.ExtremeViolationIncrease * 0.25);
        }

        return Math.Max(0.0, currentScore - config.ExtremeViolationDecay * dt);
    }
}
