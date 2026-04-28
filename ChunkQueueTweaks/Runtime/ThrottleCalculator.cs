using System;

namespace ChunkQueueTweaks;

internal sealed class ThrottleCalculator
{
    public ThrottleDecision Calculate(MovementSample sample, PressureResult pressure, double abuseScore, ChunkQueueTweaksConfig config, bool globalPressureActive)
    {
        if (sample.Speed <= config.SoftThrottleStartSpeed && !globalPressureActive)
        {
            return new ThrottleDecision(1.0);
        }

        var localPressure = Math.Max(pressure.Severity, abuseScore);
        var factor = 1.0 - localPressure * (1.0 - config.MinThrottleFactor);

        if (globalPressureActive)
        {
            factor *= config.GlobalThrottleFactor;
        }

        return new ThrottleDecision(Math.Clamp(factor, config.MinThrottleFactor, 1.0));
    }
}
