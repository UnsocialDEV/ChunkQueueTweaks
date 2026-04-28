using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class ReloadCommandHandler
{
    private readonly ChunkQueueTweaksModSystem modSystem;

    public ReloadCommandHandler(ChunkQueueTweaksModSystem modSystem)
    {
        this.modSystem = modSystem;
    }

    public TextCommandResult Handle(TextCommandCallingArgs args)
    {
        return TextCommandResult.Success(new ReloadCommandFormatter().Format(modSystem.Reload()));
    }
}
