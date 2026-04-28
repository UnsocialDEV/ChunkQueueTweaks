using System;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class ConfigLoader
{
    private const string FileName = "chunkqueuetweaks.json";

    public ChunkQueueTweaksConfig Load(ICoreServerAPI api)
    {
        try
        {
            var config = api.LoadModConfig<ChunkQueueTweaksConfig>(FileName);
            if (config is not null)
            {
                return new ConfigNormalizer().Normalize(config);
            }
        }
        catch (Exception exception)
        {
            api.Logger.Warning(exception);
        }

        var defaultConfig = new ChunkQueueTweaksConfig();
        api.StoreModConfig(defaultConfig, FileName);
        return defaultConfig;
    }
}
