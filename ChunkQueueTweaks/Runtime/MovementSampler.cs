using System;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class MovementSampler
{
    private readonly DimensionKeyCalculator _dimensionKeyCalculator = new();
    private readonly ChunkColumnCalculator _chunkColumnCalculator = new();

    public MovementSample Sample(IServerPlayer player, PlayerThrottleState state, double dt)
    {
        var pos = player.Entity.Pos;
        var history = state.MovementHistory;
        var dimensionKey = _dimensionKeyCalculator.Calculate(pos.InternalY);
        var chunkX = _chunkColumnCalculator.Calculate(pos.X);
        var chunkZ = _chunkColumnCalculator.Calculate(pos.Z);
        if (!history.HasPreviousPosition)
        {
            return new MovementSample(false, pos.X, pos.Y, pos.Z, pos.InternalY, dimensionKey, false, 0.0, chunkX, chunkZ, 0.0, 0.0, 0.0, 0.0);
        }

        var deltaX = pos.X - history.PreviousX;
        var deltaZ = pos.Z - history.PreviousZ;
        var distance = Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        var speed = dt <= 0.0 ? 0.0 : distance / dt;
        var directionX = distance <= 0.0001 ? 0.0 : deltaX / distance;
        var directionZ = distance <= 0.0001 ? 0.0 : deltaZ / distance;
        var directionDot = directionX * history.PreviousDirectionX + directionZ * history.PreviousDirectionZ;

        var dimensionChanged = dimensionKey != history.PreviousDimensionKey;

        return new MovementSample(true, pos.X, pos.Y, pos.Z, pos.InternalY, dimensionKey, dimensionChanged, distance, chunkX, chunkZ, speed, directionX, directionZ, directionDot);
    }
}
