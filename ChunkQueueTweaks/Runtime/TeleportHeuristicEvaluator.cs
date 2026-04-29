namespace ChunkQueueTweaks;

internal sealed class TeleportHeuristicEvaluator
{
    public TeleportSignal Evaluate(MovementSample sample, PlayerThrottleState state, ChunkQueueTweaksConfig config)
    {
        if (!config.AllowTeleportGraceHeuristic || state.Teleport.TeleportHeuristicCooldownMs > 0 || state.Correction.CorrectionCooldownMs > 0)
        {
            return new TeleportSignal(false, string.Empty);
        }

        if (sample.Distance < config.TeleportMinimumDistance)
        {
            return new TeleportSignal(false, string.Empty);
        }

        if (state.Pressure.ExtremeViolationScore > config.TeleportMaxRecentViolationScore || state.Pressure.AbuseScore > config.TeleportMaxRecentViolationScore)
        {
            return new TeleportSignal(false, string.Empty);
        }

        return new TeleportSignal(true, "one-shot teleport heuristic");
    }
}
