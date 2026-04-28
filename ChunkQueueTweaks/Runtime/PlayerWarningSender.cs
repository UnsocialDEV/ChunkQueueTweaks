using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerWarningSender
{
    public void Send(IServerPlayer player, PlayerThrottleState state, ChunkQueueTweaksConfig config, double totalSeconds)
    {
        if (totalSeconds - state.LastWarningTotalSeconds < config.WarningCooldownSeconds)
        {
            return;
        }

        state.LastWarningTotalSeconds = totalSeconds;
        player.SendIngameError("chunkqueuetweaks-speed", "Movement speed reduced to protect server chunk loading.");
    }
}
