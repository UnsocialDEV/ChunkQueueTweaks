using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class VelocityThrottleApplier
{
    public void Apply(IServerPlayer player, ThrottleDecision decision)
    {
        if (decision.Factor >= 0.999)
        {
            return;
        }

        var motion = player.Entity.Pos.Motion;
        motion.X *= decision.Factor;
        motion.Z *= decision.Factor;
    }
}
