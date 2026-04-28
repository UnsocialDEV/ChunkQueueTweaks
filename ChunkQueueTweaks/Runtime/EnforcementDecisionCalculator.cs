namespace ChunkQueueTweaks;

internal sealed class EnforcementDecisionCalculator
{
    public EnforcementDecision Calculate(HardMovementViolation violation, PlayerThrottleState state)
    {
        return violation.IsViolation && state.HasSafePosition
            ? new EnforcementDecision(true, violation.Reason)
            : new EnforcementDecision(false, string.Empty);
    }
}
