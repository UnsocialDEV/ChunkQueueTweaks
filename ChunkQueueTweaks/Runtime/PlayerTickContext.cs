using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal readonly record struct PlayerTickContext(IServerPlayer Player, PlayerThrottleState State);
