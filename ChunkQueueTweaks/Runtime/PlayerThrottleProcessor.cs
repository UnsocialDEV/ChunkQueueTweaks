using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerThrottleProcessor
{
    private readonly SafePositionUpdater _safePositionUpdater = new();
    private readonly PressureEvaluator _pressureEvaluator = new();
    private readonly AbuseScoreUpdater _abuseScoreUpdater = new();
    private readonly ThrottleCalculator _throttleCalculator = new();
    private readonly VelocityThrottleApplier _velocityThrottleApplier = new();
    private readonly MovementHistoryUpdater _movementHistoryUpdater = new();

    public bool Process(IServerPlayer player, PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config, GlobalPressureState globalPressure, double tickSeconds)
    {
        _safePositionUpdater.Update(state, sample, config);

        var pressure = _pressureEvaluator.Evaluate(sample, state, config, tickSeconds);
        state.Pressure.AbuseScore = _abuseScoreUpdater.Update(state.Pressure.AbuseScore, pressure, config, tickSeconds);

        var decision = _throttleCalculator.Calculate(sample, pressure, state.Pressure.AbuseScore, config, globalPressure.Active);
        _velocityThrottleApplier.Apply(player, decision);
        state.Throttle.LastThrottleFactor = decision.Factor;
        _movementHistoryUpdater.Commit(state, sample);
        return pressure.IsUnderPressure;
    }
}
