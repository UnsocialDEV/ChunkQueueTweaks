using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerTeleportProcessor
{
    private readonly TeleportSignalDetector _teleportSignalDetector = new();
    private readonly TeleportHeuristicEvaluator _teleportHeuristicEvaluator = new();
    private readonly TeleportAcceptanceHandler _teleportAcceptanceHandler = new();

    public bool ShouldContinue(IServerPlayer player, PlayerThrottleState state, MovementSample sample, ChunkQueueTweaksConfig config)
    {
        var teleportSignal = _teleportSignalDetector.Detect(player, sample);
        if (!teleportSignal.IsTeleport)
        {
            teleportSignal = _teleportHeuristicEvaluator.Evaluate(sample, state, config);
        }

        if (!teleportSignal.IsTeleport)
        {
            return true;
        }

        _teleportAcceptanceHandler.Accept(player, state, sample, config);
        return false;
    }
}
