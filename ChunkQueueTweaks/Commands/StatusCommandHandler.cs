using Vintagestory.API.Common;

namespace ChunkQueueTweaks;

internal sealed class StatusCommandHandler
{
    private readonly ChunkQueueTweaksModSystem modSystem;

    public StatusCommandHandler(ChunkQueueTweaksModSystem modSystem)
    {
        this.modSystem = modSystem;
    }

    public TextCommandResult Handle(TextCommandCallingArgs args)
    {
        return TextCommandResult.Success(new StatusCommandFormatter().Format(modSystem.Status()));
    }
}
