using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class CorrectivePositionApplier
{
    public void Apply(IServerPlayer player, PlayerThrottleState state)
    {
        var safePosition = state.SafePosition;
        player.Entity.TeleportToDouble(safePosition.LastSafeX, safePosition.LastSafeY, safePosition.LastSafeZ, null);
        var motion = player.Entity.Pos.Motion;
        motion.X = 0.0;
        motion.Z = 0.0;

        var history = state.MovementHistory;
        history.PreviousX = safePosition.LastSafeX;
        history.PreviousZ = safePosition.LastSafeZ;
        history.PreviousInternalY = safePosition.LastSafeInternalY;
        history.PreviousDimensionKey = safePosition.LastSafeDimensionKey;
        history.PreviousDirectionX = 0.0;
        history.PreviousDirectionZ = 0.0;
        state.Pressure.SustainedPressureSeconds = 0.0;
    }
}
