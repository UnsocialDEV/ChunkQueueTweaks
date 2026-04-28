namespace ChunkQueueTweaks;

internal readonly record struct MovementSample(
    bool HasPreviousPosition,
    double X,
    double Y,
    double Z,
    double InternalY,
    int DimensionKey,
    bool DimensionChanged,
    double Distance,
    int ChunkX,
    int ChunkZ,
    double Speed,
    double DirectionX,
    double DirectionZ,
    double DirectionDot);
