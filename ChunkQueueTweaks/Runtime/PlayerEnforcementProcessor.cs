using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerEnforcementProcessor
{
    private readonly HardMovementViolationEvaluator _hardMovementViolationEvaluator = new();
    private readonly ExtremeViolationScoreUpdater _extremeViolationScoreUpdater = new();
    private readonly EnforcementDecisionCalculator _enforcementDecisionCalculator = new();
    private readonly CorrectivePositionApplier _correctivePositionApplier = new();
    private readonly ChunkRadiusPressureReducer _chunkRadiusPressureReducer = new();
    private readonly PlayerWarningSender _playerWarningSender = new();

    public EnforcementProcessResult Process(IServerPlayer player, PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config, double tickSeconds)
    {
        var hardViolation = _hardMovementViolationEvaluator.Evaluate(sample, state, config);
        state.Pressure.ExtremeViolationScore = _extremeViolationScoreUpdater.Update(state.Pressure.ExtremeViolationScore, sample, hardViolation, config, tickSeconds);
        if (!hardViolation.IsViolation && sample.Speed > config.MaxSafeHorizontalSpeed && state.Pressure.ExtremeViolationScore >= config.MaxExtremeViolationScore)
        {
            hardViolation = new HardMovementViolation(true, "repeated high-speed movement");
        }

        var enforcement = _enforcementDecisionCalculator.Calculate(hardViolation, state);
        if (!enforcement.ShouldCorrect)
        {
            return new EnforcementProcessResult(true);
        }

        _correctivePositionApplier.Apply(player, state);
        _chunkRadiusPressureReducer.Reduce(player, config);
        _playerWarningSender.Send(player, state, config, state.Observation.ObservedTotalSeconds);
        state.Correction.CorrectionCooldownMs = config.CorrectionCooldownMs;
        state.Correction.CorrectedLastTick = true;
        state.Throttle.LastThrottleFactor = 0.0;
        return new EnforcementProcessResult(false);
    }
}
