using System;

namespace ChunkQueueTweaks;

internal sealed class ConfigNormalizer
{
    public ChunkQueueTweaksConfig Normalize(ChunkQueueTweaksConfig config)
    {
        config.TickIntervalMs = Math.Clamp(config.TickIntervalMs, 50, 5000);
        config.MaxSafeHorizontalSpeed = Math.Max(1.0, config.MaxSafeHorizontalSpeed);
        config.SoftThrottleStartSpeed = Math.Clamp(config.SoftThrottleStartSpeed, 1.0, config.MaxSafeHorizontalSpeed);
        config.HardMaxHorizontalSpeed = Math.Max(config.MaxSafeHorizontalSpeed + 1.0, config.HardMaxHorizontalSpeed);
        config.ExtremeSpeed = Math.Max(config.MaxSafeHorizontalSpeed + 1.0, config.ExtremeSpeed);
        config.MinThrottleFactor = Math.Clamp(config.MinThrottleFactor, 0.05, 1.0);
        config.AbuseIncreasePerSecond = Math.Max(0.0, config.AbuseIncreasePerSecond);
        config.AbuseDecayPerSecond = Math.Max(0.0, config.AbuseDecayPerSecond);
        config.PressureDirectionDotThreshold = Math.Clamp(config.PressureDirectionDotThreshold, 0.0, 1.0);
        config.SustainedPressureSeconds = Math.Max(0.0, config.SustainedPressureSeconds);
        config.GlobalPressurePlayerCount = Math.Max(1, config.GlobalPressurePlayerCount);
        config.GlobalThrottleFactor = Math.Clamp(config.GlobalThrottleFactor, config.MinThrottleFactor, 1.0);
        config.ExemptPrivilege = string.IsNullOrWhiteSpace(config.ExemptPrivilege) ? "controlserver" : config.ExemptPrivilege;
        config.MaxChunkColumnsPerTick = Math.Max(1, config.MaxChunkColumnsPerTick);
        config.CorrectionCooldownMs = Math.Clamp(config.CorrectionCooldownMs, 0, 10000);
        config.ExtremeViolationIncrease = Math.Max(0.0, config.ExtremeViolationIncrease);
        config.ExtremeViolationDecay = Math.Max(0.0, config.ExtremeViolationDecay);
        config.MaxExtremeViolationScore = Math.Max(0.1, config.MaxExtremeViolationScore);
        config.CorrectiveChunkSentRadius = Math.Max(0, config.CorrectiveChunkSentRadius);
        config.WarningCooldownSeconds = Math.Max(0.0, config.WarningCooldownSeconds);
        config.TeleportGraceCooldownMs = Math.Clamp(config.TeleportGraceCooldownMs, 0, 60000);
        config.TeleportArrivalGraceMs = Math.Clamp(config.TeleportArrivalGraceMs, 0, 10000);
        config.TeleportMinimumDistance = Math.Max(1.0, config.TeleportMinimumDistance);
        config.TeleportMaxRecentViolationScore = Math.Clamp(config.TeleportMaxRecentViolationScore, 0.0, config.MaxExtremeViolationScore);
        return config;
    }
}
