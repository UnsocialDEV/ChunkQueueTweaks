using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerWarningSender
{
    public void Send(IServerPlayer player, PlayerThrottleState state, ChunkQueueTweaksConfig config, double totalSeconds)
    {
        var correction = state.Correction;
        if (totalSeconds - correction.LastWarningTotalSeconds < config.WarningCooldownSeconds)
        {
            return;
        }

        correction.LastWarningTotalSeconds = totalSeconds;
        player.SendIngameError("chunkqueuetweaks-speed", "Movement speed reduced to protect server chunk loading.");
    }
}
