using System;

namespace ChunkQueueTweaks;

internal sealed class ChunkColumnCalculator
{
    private const int ChunkSize = 32;

    public int Calculate(double blockCoordinate)
    {
        return (int)Math.Floor(blockCoordinate / ChunkSize);
    }
}
