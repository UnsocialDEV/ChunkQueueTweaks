using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class CommandRegistrar
{
    private readonly ChunkQueueTweaksModSystem modSystem;

    public CommandRegistrar(ChunkQueueTweaksModSystem modSystem)
    {
        this.modSystem = modSystem;
    }

    public void Register(ICoreServerAPI api)
    {
        api.ChatCommands.Create("chunkqueuetweaks")
            .WithDescription("Inspect or reload ChunkQueueTweaks.")
            .RequiresPrivilege(Privilege.controlserver)
            .BeginSubCommand("status")
                .WithDescription("Show current ChunkQueueTweaks status.")
                .HandleWith(new StatusCommandHandler(modSystem).Handle)
                .EndSubCommand()
            .BeginSubCommand("reload")
                .WithDescription("Reload ChunkQueueTweaks config.")
                .HandleWith(new ReloadCommandHandler(modSystem).Handle)
                .EndSubCommand();
    }
}
