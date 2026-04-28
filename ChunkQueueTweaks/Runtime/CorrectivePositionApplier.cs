using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class CorrectivePositionApplier
{
    public void Apply(IServerPlayer player, PlayerThrottleState state)
    {
        player.Entity.TeleportToDouble(state.LastSafeX, state.LastSafeY, state.LastSafeZ, null);
        var motion = player.Entity.Pos.Motion;
        motion.X = 0.0;
        motion.Z = 0.0;
        state.PreviousX = state.LastSafeX;
        state.PreviousZ = state.LastSafeZ;
        state.PreviousInternalY = state.LastSafeInternalY;
        state.PreviousDimensionKey = state.LastSafeDimensionKey;
        state.PreviousDirectionX = 0.0;
        state.PreviousDirectionZ = 0.0;
        state.SustainedPressureSeconds = 0.0;
    }
}
