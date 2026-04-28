using System;
using System.Collections.Generic;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TickProcessor
{
    public GlobalPressureState Process(ICoreServerAPI api, Dictionary<string, PlayerThrottleState> states, ChunkQueueTweaksConfig config, GlobalPressureState globalPressure, float dt)
    {
        var players = api.World.AllOnlinePlayers;
        new PlayerStateCleaner().Clean(states, players);

        int pressuredPlayers = 0;
        var tickSeconds = Math.Max(dt, config.TickIntervalMs / 1000f);

        foreach (var player in players)
        {
            if (player is not IServerPlayer serverPlayer || !new PlayerEligibility().IsEligible(serverPlayer, config))
            {
                continue;
            }

            var state = new PlayerStateProvider().Get(states, serverPlayer);
            state.CorrectedLastTick = false;
            state.ObservedTotalSeconds += tickSeconds;
            new CorrectionClock().Tick(state, tickSeconds);
            new TeleportGraceClock().Tick(state, tickSeconds);

            var sample = new MovementSampler().Sample(serverPlayer, state, tickSeconds);
            if (!sample.HasPreviousPosition)
            {
                new SafePositionUpdater().Update(state, sample, config);
                continue;
            }

            var teleportSignal = new TeleportSignalDetector().Detect(serverPlayer, sample);
            if (!teleportSignal.IsTeleport)
            {
                teleportSignal = new TeleportHeuristicEvaluator().Evaluate(sample, state, config);
            }

            if (teleportSignal.IsTeleport)
            {
                new TeleportAcceptanceHandler().Accept(serverPlayer, state, sample, config);
                continue;
            }

            var hardViolation = new HardMovementViolationEvaluator().Evaluate(sample, state, config);
            state.ExtremeViolationScore = new ExtremeViolationScoreUpdater().Update(state.ExtremeViolationScore, sample, hardViolation, config, tickSeconds);
            hardViolation = new HardMovementViolationEvaluator().Evaluate(sample, state, config);
            var enforcement = new EnforcementDecisionCalculator().Calculate(hardViolation, state);

            if (enforcement.ShouldCorrect)
            {
                new CorrectivePositionApplier().Apply(serverPlayer, state);
                new ChunkRadiusPressureReducer().Reduce(serverPlayer, config);
                new PlayerWarningSender().Send(serverPlayer, state, config, state.ObservedTotalSeconds);
                state.CorrectionCooldownMs = config.CorrectionCooldownMs;
                state.CorrectedLastTick = true;
                state.LastThrottleFactor = 0.0;
                continue;
            }

            new SafePositionUpdater().Update(state, sample, config);

            var pressure = new PressureEvaluator().Evaluate(sample, state, config, tickSeconds);
            state.AbuseScore = new AbuseScoreUpdater().Update(state.AbuseScore, pressure, config, tickSeconds);

            if (pressure.IsUnderPressure)
            {
                pressuredPlayers++;
            }

            var decision = new ThrottleCalculator().Calculate(sample, pressure, state.AbuseScore, config, globalPressure.Active);
            new VelocityThrottleApplier().Apply(serverPlayer, decision);
            state.LastThrottleFactor = decision.Factor;
        }

        return new GlobalPressureState(new GlobalPressureEvaluator().Evaluate(pressuredPlayers, config));
    }
}
