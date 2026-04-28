using System;

namespace ChunkQueueTweaks;

internal sealed class AbuseScoreUpdater
{
    public double Update(double currentScore, PressureResult pressure, ChunkQueueTweaksConfig config, double dt)
    {
        if (pressure.IsUnderPressure)
        {
            return Math.Clamp(currentScore + config.AbuseIncreasePerSecond * dt * (1.0 + pressure.Severity), 0.0, 1.0);
        }

        return Math.Clamp(currentScore - config.AbuseDecayPerSecond * dt, 0.0, 1.0);
    }
}
