namespace ChunkQueueTweaks;

internal readonly record struct EnforcementDecision(bool ShouldCorrect, string Reason);
