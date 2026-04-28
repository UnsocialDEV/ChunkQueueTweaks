using System;
using System.Collections.Generic;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TickProcessor
{
    private const double CleanupIntervalSeconds = 5.0;

    private readonly PlayerStateCleaner _playerStateCleaner = new();
    private readonly PlayerEligibility _playerEligibility = new();
    private readonly PlayerStateProvider _playerStateProvider = new();
    private readonly CorrectionClock _correctionClock = new();
    private readonly TeleportGraceClock _teleportGraceClock = new();
    private readonly MovementSampler _movementSampler = new();
    private readonly SafePositionUpdater _safePositionUpdater = new();
    private readonly TeleportSignalDetector _teleportSignalDetector = new();
    private readonly TeleportHeuristicEvaluator _teleportHeuristicEvaluator = new();
    private readonly TeleportAcceptanceHandler _teleportAcceptanceHandler = new();
    private readonly HardMovementViolationEvaluator _hardMovementViolationEvaluator = new();
    private readonly ExtremeViolationScoreUpdater _extremeViolationScoreUpdater = new();
    private readonly EnforcementDecisionCalculator _enforcementDecisionCalculator = new();
    private readonly CorrectivePositionApplier _correctivePositionApplier = new();
    private readonly ChunkRadiusPressureReducer _chunkRadiusPressureReducer = new();
    private readonly PlayerWarningSender _playerWarningSender = new();
    private readonly PressureEvaluator _pressureEvaluator = new();
    private readonly AbuseScoreUpdater _abuseScoreUpdater = new();
    private readonly ThrottleCalculator _throttleCalculator = new();
    private readonly VelocityThrottleApplier _velocityThrottleApplier = new();
    private readonly GlobalPressureEvaluator _globalPressureEvaluator = new();
    private double _cleanupElapsedSeconds = CleanupIntervalSeconds;

    public GlobalPressureState Process(ICoreServerAPI api, Dictionary<string, PlayerThrottleState> states, ChunkQueueTweaksConfig config, GlobalPressureState globalPressure, float dt)
    {
        var players = api.World.AllOnlinePlayers;
        int pressuredPlayers = 0;
        var tickSeconds = Math.Max(dt, config.TickIntervalMs / 1000f);

        _cleanupElapsedSeconds += tickSeconds;
        if (_cleanupElapsedSeconds >= CleanupIntervalSeconds)
        {
            _playerStateCleaner.Clean(states, players);
            _cleanupElapsedSeconds = 0.0;
        }

        foreach (var player in players)
        {
            if (player is not IServerPlayer serverPlayer || !_playerEligibility.IsEligible(serverPlayer, config))
            {
                continue;
            }

            var state = _playerStateProvider.Get(states, serverPlayer);
            state.CorrectedLastTick = false;
            state.ObservedTotalSeconds += tickSeconds;
            _correctionClock.Tick(state, tickSeconds);
            _teleportGraceClock.Tick(state, tickSeconds);

            var sample = _movementSampler.Sample(serverPlayer, state, tickSeconds);
            if (!sample.HasPreviousPosition)
            {
                _safePositionUpdater.Update(state, sample, config);
                continue;
            }

            var teleportSignal = _teleportSignalDetector.Detect(serverPlayer, sample);
            if (!teleportSignal.IsTeleport)
            {
                teleportSignal = _teleportHeuristicEvaluator.Evaluate(sample, state, config);
            }

            if (teleportSignal.IsTeleport)
            {
                _teleportAcceptanceHandler.Accept(serverPlayer, state, sample, config);
                continue;
            }

            var hardViolation = _hardMovementViolationEvaluator.Evaluate(sample, state, config);
            state.ExtremeViolationScore = _extremeViolationScoreUpdater.Update(state.ExtremeViolationScore, sample, hardViolation, config, tickSeconds);
            if (!hardViolation.IsViolation && sample.Speed > config.MaxSafeHorizontalSpeed && state.ExtremeViolationScore >= config.MaxExtremeViolationScore)
            {
                hardViolation = new HardMovementViolation(true, "repeated high-speed movement");
            }

            var enforcement = _enforcementDecisionCalculator.Calculate(hardViolation, state);

            if (enforcement.ShouldCorrect)
            {
                _correctivePositionApplier.Apply(serverPlayer, state);
                _chunkRadiusPressureReducer.Reduce(serverPlayer, config);
                _playerWarningSender.Send(serverPlayer, state, config, state.ObservedTotalSeconds);
                state.CorrectionCooldownMs = config.CorrectionCooldownMs;
                state.CorrectedLastTick = true;
                state.LastThrottleFactor = 0.0;
                continue;
            }

            _safePositionUpdater.Update(state, sample, config);

            var pressure = _pressureEvaluator.Evaluate(sample, state, config, tickSeconds);
            state.AbuseScore = _abuseScoreUpdater.Update(state.AbuseScore, pressure, config, tickSeconds);

            if (pressure.IsUnderPressure)
            {
                pressuredPlayers++;
            }

            var decision = _throttleCalculator.Calculate(sample, pressure, state.AbuseScore, config, globalPressure.Active);
            _velocityThrottleApplier.Apply(serverPlayer, decision);
            state.LastThrottleFactor = decision.Factor;
        }

        return new GlobalPressureState(_globalPressureEvaluator.Evaluate(pressuredPlayers, config));
    }
}
