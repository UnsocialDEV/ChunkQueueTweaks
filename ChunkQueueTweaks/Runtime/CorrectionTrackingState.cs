namespace ChunkQueueTweaks;

internal sealed class CorrectionTrackingState
{
    public int CorrectionCooldownMs;
    public double LastWarningTotalSeconds = -9999.0;
    public bool CorrectedLastTick;
}
