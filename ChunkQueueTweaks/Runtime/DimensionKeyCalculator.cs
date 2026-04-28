using System;

namespace ChunkQueueTweaks;

internal sealed class DimensionKeyCalculator
{
    private const int DimensionBoundary = 32768;

    public int Calculate(double internalY)
    {
        return (int)Math.Floor(internalY / DimensionBoundary);
    }
}
