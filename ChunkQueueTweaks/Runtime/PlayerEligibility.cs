using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class PlayerEligibility
{
    public bool IsEligible(IServerPlayer player, ChunkQueueTweaksConfig config)
    {
        return player.ConnectionState == EnumClientState.Playing
            && player.Entity is not null
            && !player.HasPrivilege(config.ExemptPrivilege);
    }
}
