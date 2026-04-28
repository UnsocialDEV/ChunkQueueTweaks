using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class TeleportAcceptanceHandler
{
    public void Accept(IServerPlayer player, PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config)
    {
        var motion = player.Entity.Pos.Motion;
        motion.X = 0.0;
        motion.Z = 0.0;
        state.PreviousX = sample.X;
        state.PreviousZ = sample.Z;
        state.PreviousInternalY = sample.InternalY;
        state.PreviousDimensionKey = sample.DimensionKey;
        state.PreviousDirectionX = 0.0;
        state.PreviousDirectionZ = 0.0;
        state.SustainedPressureSeconds = 0.0;
        state.AbuseScore = 0.0;
        state.ExtremeViolationScore = 0.0;
        state.CorrectionCooldownMs = 0;
        state.TeleportGraceMs = config.TeleportArrivalGraceMs;
        state.TeleportHeuristicCooldownMs = config.TeleportGraceCooldownMs;
        state.LastAcceptedTeleportTotalSeconds = state.ObservedTotalSeconds;
        state.AcceptedTeleportCount++;
        state.LastThrottleFactor = 1.0;
        state.CorrectedLastTick = false;
        new SafePositionWriter().Write(state, sample);
    }
}
