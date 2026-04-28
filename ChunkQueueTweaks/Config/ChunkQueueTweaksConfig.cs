namespace ChunkQueueTweaks;

public sealed class ChunkQueueTweaksConfig
{
    public bool Enabled = true;
    public int TickIntervalMs = 250;
    public double MaxSafeHorizontalSpeed = 18.0;
    public double SoftThrottleStartSpeed = 14.0;
    public double HardMaxHorizontalSpeed = 36.0;
    public double ExtremeSpeed = 45.0;
    public double MinThrottleFactor = 0.25;
    public double AbuseIncreasePerSecond = 0.22;
    public double AbuseDecayPerSecond = 0.12;
    public double PressureDirectionDotThreshold = 0.94;
    public double SustainedPressureSeconds = 1.0;
    public int GlobalPressurePlayerCount = 3;
    public double GlobalThrottleFactor = 0.88;
    public string ExemptPrivilege = "controlserver";
    public int MaxChunkColumnsPerTick = 1;
    public int CorrectionCooldownMs = 750;
    public double ExtremeViolationIncrease = 0.35;
    public double ExtremeViolationDecay = 0.18;
    public double MaxExtremeViolationScore = 1.0;
    public bool ReduceChunkRadiusOnCorrection = true;
    public int CorrectiveChunkSentRadius = 1;
    public double WarningCooldownSeconds = 4.0;
    public bool AllowTeleportGraceHeuristic = true;
    public int TeleportGraceCooldownMs = 5000;
    public int TeleportArrivalGraceMs = 1000;
    public double TeleportMinimumDistance = 64.0;
    public double TeleportMaxRecentViolationScore = 0.0;
}
