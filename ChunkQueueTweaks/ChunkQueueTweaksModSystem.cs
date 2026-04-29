using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ChunkQueueTweaks;

public sealed class ChunkQueueTweaksModSystem : ModSystem
{
    private readonly Dictionary<string, PlayerThrottleState> states = new();
    private readonly ConfigLoader configLoader = new();
    private readonly CommandRegistrar commandRegistrar;
    private readonly TickProcessor tickProcessor = new();
    private readonly ThrottledPlayerCounter throttledPlayerCounter = new();
    private readonly CorrectedPlayerCounter correctedPlayerCounter = new();
    private readonly TeleportGraceCounter teleportGraceCounter = new();
    private ICoreServerAPI? api;
    private ChunkQueueTweaksConfig config = new();
    private long listenerId;
    private GlobalPressureState globalPressure = new();

    public ChunkQueueTweaksModSystem()
    {
        commandRegistrar = new CommandRegistrar(this);
    }

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Server;
    }

    public override void StartServerSide(ICoreServerAPI sapi)
    {
        api = sapi;
        config = configLoader.Load(sapi);
        listenerId = sapi.Event.RegisterGameTickListener(OnTick, OnTickError, config.TickIntervalMs);
        sapi.Event.PlayerDisconnect += OnPlayerDisconnect;
        commandRegistrar.Register(sapi);
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
            throttledPlayerCounter.Count(states.Values),
            correctedPlayerCounter.Count(states.Values),
            config.HardMaxHorizontalSpeed,
            config.MaxChunkColumnsPerTick,
            teleportGraceCounter.Count(states.Values),
            config.AllowTeleportGraceHeuristic);
    }

    internal bool Reload()
    {
        if (api is null)
        {
            return false;
        }

        config = configLoader.Load(api);
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

        globalPressure = tickProcessor.Process(api, states, config, globalPressure, dt);
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
