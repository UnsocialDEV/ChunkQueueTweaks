using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TeleportAcceptanceHandler
{
    private readonly SafePositionWriter _safePositionWriter = new();

    public void Accept(IServerPlayer player, PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config)
    {
        var motion = player.Entity.Pos.Motion;
        motion.X = 0.0;
        motion.Z = 0.0;

        var history = state.MovementHistory;
        history.PreviousX = sample.X;
        history.PreviousZ = sample.Z;
        history.PreviousInternalY = sample.InternalY;
        history.PreviousDimensionKey = sample.DimensionKey;
        history.PreviousDirectionX = 0.0;
        history.PreviousDirectionZ = 0.0;

        state.Pressure.SustainedPressureSeconds = 0.0;
        state.Pressure.AbuseScore = 0.0;
        state.Pressure.ExtremeViolationScore = 0.0;
        state.Correction.CorrectionCooldownMs = 0;
        state.Correction.CorrectedLastTick = false;
        state.Teleport.TeleportGraceMs = config.TeleportArrivalGraceMs;
        state.Teleport.TeleportHeuristicCooldownMs = config.TeleportGraceCooldownMs;
        state.Teleport.LastAcceptedTeleportTotalSeconds = state.Observation.ObservedTotalSeconds;
        state.Teleport.AcceptedTeleportCount++;
        state.Throttle.LastThrottleFactor = 1.0;
        _safePositionWriter.Write(state, sample);
    }
}
