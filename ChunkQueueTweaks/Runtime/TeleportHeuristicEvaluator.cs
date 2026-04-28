namespace ChunkQueueTweaks;

internal sealed class TeleportHeuristicEvaluator
{
    public TeleportSignal Evaluate(MovementSample sample, PlayerThrottleState state, ChunkQueueTweaksConfig config)
    {
        if (!config.AllowTeleportGraceHeuristic || state.TeleportHeuristicCooldownMs > 0 || state.CorrectionCooldownMs > 0)
        {
            return new TeleportSignal(false, string.Empty);
        }

        if (sample.Distance < config.TeleportMinimumDistance)
        {
            return new TeleportSignal(false, string.Empty);
        }

        if (state.ExtremeViolationScore > config.TeleportMaxRecentViolationScore || state.AbuseScore > config.TeleportMaxRecentViolationScore)
        {
            return new TeleportSignal(false, string.Empty);
        }

        return new TeleportSignal(true, "one-shot teleport heuristic");
    }
}
