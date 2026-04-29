namespace ChunkQueueTweaks;

internal readonly record struct MovementObservation(MovementSample Sample, bool ShouldContinue);
