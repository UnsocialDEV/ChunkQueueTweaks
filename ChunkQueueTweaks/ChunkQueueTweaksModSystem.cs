using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

public sealed class ChunkQueueTweaksModSystem : ModSystem
{
    private readonly Dictionary<string, PlayerThrottleState> states = new();
    private ICoreServerAPI? api;
    private ChunkQueueTweaksConfig config = new();
    private long listenerId;
    private GlobalPressureState globalPressure = new();

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Server;
    }

    public override void StartServerSide(ICoreServerAPI sapi)
    {
        api = sapi;
        config = new ConfigLoader().Load(sapi);
        listenerId = sapi.Event.RegisterGameTickListener(OnTick, OnTickError, config.TickIntervalMs);
        sapi.Event.PlayerDisconnect += OnPlayerDisconnect;
        new CommandRegistrar(this).Register(sapi);
        sapi.Logger.Notification("ChunkQueueTweaks loaded with {0}ms tick interval.", config.TickIntervalMs);
    }

    public override void Dispose()
    {
        if (api is null)
        {
            return;
        }

        api.Event.UnregisterGameTickListener(listenerId);
        api.Event.PlayerDisconnect -= OnPlayerDisconnect;
    }

    internal CommandStatusSnapshot Status()
    {
        return new CommandStatusSnapshot(
            config.Enabled,
            states.Count,
            globalPressure.Active,
            new ThrottledPlayerCounter().Count(states.Values),
            new CorrectedPlayerCounter().Count(states.Values),
            config.HardMaxHorizontalSpeed,
            config.MaxChunkColumnsPerTick,
            new TeleportGraceCounter().Count(states.Values),
            config.AllowTeleportGraceHeuristic);
    }

    internal bool Reload()
    {
        if (api is null)
        {
            return false;
        }

        config = new ConfigLoader().Load(api);
        api.Event.UnregisterGameTickListener(listenerId);
        listenerId = api.Event.RegisterGameTickListener(OnTick, OnTickError, config.TickIntervalMs);
        return true;
    }

    private void OnTick(float dt)
    {
        if (api is null || !config.Enabled)
        {
            return;
        }

        globalPressure = new TickProcessor().Process(api, states, config, globalPressure, dt);
    }

    private void OnTickError(Exception exception)
    {
        api?.Logger.Error(exception);
    }

    private void OnPlayerDisconnect(IServerPlayer player)
    {
        states.Remove(player.PlayerUID);
    }
}
