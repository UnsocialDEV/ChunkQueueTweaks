using System;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class MovementSampler
{
    public MovementSample Sample(IServerPlayer player, PlayerThrottleState state, double dt)
    {
        var pos = player.Entity.Pos;
        var dimensionKey = new DimensionKeyCalculator().Calculate(pos.InternalY);
        if (!state.HasPreviousPosition)
        {
            state.HasPreviousPosition = true;
            state.PreviousX = pos.X;
            state.PreviousZ = pos.Z;
            state.PreviousInternalY = pos.InternalY;
            state.PreviousDimensionKey = dimensionKey;
            return new MovementSample(false, pos.X, pos.Y, pos.Z, pos.InternalY, dimensionKey, false, 0.0, new ChunkColumnCalculator().Calculate(pos.X), new ChunkColumnCalculator().Calculate(pos.Z), 0.0, 0.0, 0.0, 0.0);
        }

        var deltaX = pos.X - state.PreviousX;
        var deltaZ = pos.Z - state.PreviousZ;
        var distance = Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        var speed = dt <= 0.0 ? 0.0 : distance / dt;
        var directionX = distance <= 0.0001 ? 0.0 : deltaX / distance;
        var directionZ = distance <= 0.0001 ? 0.0 : deltaZ / distance;
        var directionDot = directionX * state.PreviousDirectionX + directionZ * state.PreviousDirectionZ;

        state.PreviousX = pos.X;
        state.PreviousZ = pos.Z;
        state.PreviousInternalY = pos.InternalY;
        var dimensionChanged = dimensionKey != state.PreviousDimensionKey;
        state.PreviousDimensionKey = dimensionKey;
        state.PreviousDirectionX = directionX;
        state.PreviousDirectionZ = directionZ;

        return new MovementSample(true, pos.X, pos.Y, pos.Z, pos.InternalY, dimensionKey, dimensionChanged, distance, new ChunkColumnCalculator().Calculate(pos.X), new ChunkColumnCalculator().Calculate(pos.Z), speed, directionX, directionZ, directionDot);
    }
}
