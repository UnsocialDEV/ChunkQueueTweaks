using System;

namespace ChunkQueueTweaks;

internal sealed class PressureEvaluator
{
    public PressureResult Evaluate(MovementSample sample, PlayerThrottleState state, ChunkQueueTweaksConfig config, double dt)
    {
        var overSafeSpeed = sample.Speed > config.MaxSafeHorizontalSpeed;
        var linear = sample.DirectionDot >= config.PressureDirectionDotThreshold;
        state.SustainedPressureSeconds = overSafeSpeed && linear
            ? state.SustainedPressureSeconds + dt
            : Math.Max(0.0, state.SustainedPressureSeconds - dt);

        var sustained = state.SustainedPressureSeconds >= config.SustainedPressureSeconds;
        var underPressure = overSafeSpeed && sustained;
        var severity = Math.Clamp((sample.Speed - config.SoftThrottleStartSpeed) / (config.ExtremeSpeed - config.SoftThrottleStartSpeed), 0.0, 1.0);
        return new PressureResult(overSafeSpeed, sustained, underPressure, severity);
    }
}
