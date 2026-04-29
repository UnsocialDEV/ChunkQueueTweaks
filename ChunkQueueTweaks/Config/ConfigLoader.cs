using System;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

internal sealed class ConfigLoader
{
    private const string FileName = "chunkqueuetweaks.json";

    private readonly ConfigNormalizer _configNormalizer = new();

    public ChunkQueueTweaksConfig Load(ICoreServerAPI api)
    {
        try
        {
            var config = api.LoadModConfig<ChunkQueueTweaksConfig>(FileName);
            if (config is not null)
            {
                return _configNormalizer.Normalize(config);
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
