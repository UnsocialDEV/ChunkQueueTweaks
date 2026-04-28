namespace ChunkQueueTweaks;

internal sealed class PlayerThrottleState
{
    public bool HasPreviousPosition;
    public double PreviousX;
    public double PreviousZ;
    public double PreviousInternalY;
    public int PreviousDimensionKey;
    public double PreviousDirectionX;
    public double PreviousDirectionZ;
    public double SustainedPressureSeconds;
    public double AbuseScore;
    public double LastThrottleFactor = 1.0;
    public bool HasSafePosition;
    public double LastSafeX;
    public double LastSafeY;
    public double LastSafeZ;
    public double LastSafeInternalY;
    public int LastSafeDimensionKey;
    public int LastSafeChunkX;
    public int LastSafeChunkZ;
    public int CorrectionCooldownMs;
    public double ExtremeViolationScore;
    public double LastWarningTotalSeconds = -9999.0;
    public bool CorrectedLastTick;
    public double ObservedTotalSeconds;
    public int TeleportGraceMs;
    public int TeleportHeuristicCooldownMs;
    public double LastAcceptedTeleportTotalSeconds = -9999.0;
    public int AcceptedTeleportCount;
}
