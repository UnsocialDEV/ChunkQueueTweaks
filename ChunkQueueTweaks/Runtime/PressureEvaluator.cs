using System;

namespace ChunkQueueTweaks;

internal sealed class PressureEvaluator
{
    public PressureResult Evaluate(MovementSample sample, PlayerThrottleState state, ChunkQueueTweaksConfig config, double dt)
    {
        var overSafeSpeed = sample.Speed > config.MaxSafeHorizontalSpeed;
        var linear = sample.DirectionDot >= config.PressureDirectionDotThreshold;
        var pressure = state.Pressure;
        pressure.SustainedPressureSeconds = overSafeSpeed && linear
            ? pressure.SustainedPressureSeconds + dt
            : Math.Max(0.0, pressure.SustainedPressureSeconds - dt);

        var sustained = pressure.SustainedPressureSeconds >= config.SustainedPressureSeconds;
        var underPressure = overSafeSpeed && sustained;
        var severity = Math.Clamp((sample.Speed - config.SoftThrottleStartSpeed) / (config.ExtremeSpeed - config.SoftThrottleStartSpeed), 0.0, 1.0);
        return new PressureResult(overSafeSpeed, sustained, underPressure, severity);
    }
}
