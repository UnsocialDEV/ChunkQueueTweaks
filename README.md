# ChunkQueueTweaks

ChunkQueueTweaks is a lightweight server-side Vintage Story mod that protects servers from chunk loading queue pressure caused by players moving faster than the server can safely generate and send terrain.

It is intended for public or semi-public servers where hacked speed clients, extreme flight, repeated high-speed movement, or aggressive chunk streaming can cause instability such as `Indexed Fifo Queue overflow`.

The mod does not require client installation.

## Features

- Server-side only protection.
- Soft throttling for high-speed but non-exploit movement.
- Hard correction for exploit-level movement.
- Last safe server-approved position tracking.
- Chunk-column jump detection.
- Repeated high-speed abuse tracking.
- Legitimate teleport support for Vintage Story teleport events, dimension changes, translocators, and admin teleports.
- Admin exemption by privilege.
- Runtime config reload command.
- Lightweight tick loop with no chunk dictionary scans.

## Current Behavior

ChunkQueueTweaks samples each eligible online player at a configurable interval, defaulting to every `250ms`.

For each player, it tracks:

- Previous sampled position.
- Horizontal X/Z speed.
- Movement direction consistency.
- Last safe server-approved position.
- Last safe chunk column.
- Abuse score.
- Extreme movement violation score.
- Correction cooldown.
- Teleport grace/cooldown state.

Normal movement is ignored. Moderate high-speed movement can receive soft resistance. Exploit-level movement is corrected back to the last safe position already observed by the server.

## What Counts As Normal Movement

The mod is designed not to affect:

- Walking.
- Sprinting.
- Normal jumping and falling.
- Regular terrain traversal.
- Legitimate one-shot teleports.
- Admin movement when the admin has the configured exemption privilege.

Vertical movement alone is not treated as chunk pressure. The mod focuses on horizontal X/Z movement because that is what drives new chunk columns.

## What Counts As Pressure

A player starts creating movement pressure when they exceed the configured safe horizontal speed and continue moving in a consistent direction.

Pressure can cause:

- A soft horizontal velocity reduction.
- Gradual abuse score increase.
- Stronger throttling if the behavior continues.

This layer is meant for fast but not necessarily malicious movement.

## What Counts As Exploit Movement

The mod applies hard correction when movement looks like it can generate chunks faster than the server can handle.

Examples:

- Horizontal speed exceeds `HardMaxHorizontalSpeed`.
- A player jumps more chunk columns than `MaxChunkColumnsPerTick`.
- A player repeatedly moves above safe speed until their extreme violation score reaches the configured limit.

When hard correction happens:

1. The player is moved back to their last safe server-approved position.
2. Horizontal motion is cleared.
3. The player enters correction cooldown.
4. Their current chunk sent radius can be reduced.
5. A throttled warning can be shown to the player.

## Teleport Handling

Legitimate teleports are allowed before hard movement checks run.

The mod accepts teleport arrivals from:

- Vintage Story engine teleport flags: `Entity.Teleporting` and `Entity.IsTeleport`.
- Dimension changes.
- A conservative one-shot teleport heuristic for legitimate server-side teleports that do not expose a dedicated engine signal.

When a teleport is accepted:

- The arrival position becomes the new safe position.
- Previous movement sampling is reset to the arrival position.
- Abuse and hard violation scores are cleared.
- Correction cooldown is cleared.
- The accepted teleport tick is not throttled or corrected.

Repeated large jumps are still restricted. The heuristic has a cooldown so it cannot be used as continuous hacked movement.

## Admin Exemption

Players with the configured exemption privilege are ignored by the mod.

Default:

```json
"ExemptPrivilege": "controlserver"
```

This means server admins with `controlserver` can teleport and move freely for moderation and testing.

## Commands

Commands require the `controlserver` privilege.

### Status

```text
/chunkqueuetweaks status
```

Shows runtime state, including:

- Whether the mod is enabled.
- Number of tracked players.
- Number of throttled players.
- Number of corrected players.
- Number of players currently in teleport grace.
- Whether teleport heuristic support is enabled.
- Global pressure state.
- Hard movement limits.

### Reload

```text
/chunkqueuetweaks reload
```

Reloads `chunkqueuetweaks.json` without restarting the server.

## Configuration

The config file is stored in the server ModConfig folder:

```text
VintagestoryData\ModConfig\chunkqueuetweaks.json
```

The file is created automatically with defaults if it does not exist.

### Example Config

```json
{
  "Enabled": true,
  "TickIntervalMs": 250,
  "MaxSafeHorizontalSpeed": 18.0,
  "SoftThrottleStartSpeed": 14.0,
  "HardMaxHorizontalSpeed": 36.0,
  "ExtremeSpeed": 45.0,
  "MinThrottleFactor": 0.25,
  "AbuseIncreasePerSecond": 0.22,
  "AbuseDecayPerSecond": 0.12,
  "PressureDirectionDotThreshold": 0.94,
  "SustainedPressureSeconds": 1.0,
  "GlobalPressurePlayerCount": 3,
  "GlobalThrottleFactor": 0.88,
  "ExemptPrivilege": "controlserver",
  "MaxChunkColumnsPerTick": 1,
  "CorrectionCooldownMs": 750,
  "ExtremeViolationIncrease": 0.35,
  "ExtremeViolationDecay": 0.18,
  "MaxExtremeViolationScore": 1.0,
  "ReduceChunkRadiusOnCorrection": true,
  "CorrectiveChunkSentRadius": 1,
  "WarningCooldownSeconds": 4.0,
  "AllowTeleportGraceHeuristic": true,
  "TeleportGraceCooldownMs": 5000,
  "TeleportArrivalGraceMs": 1000,
  "TeleportMinimumDistance": 64.0,
  "TeleportMaxRecentViolationScore": 0.0
}
```

### Config Reference

| Setting | Purpose |
| --- | --- |
| `Enabled` | Enables or disables all runtime enforcement. |
| `TickIntervalMs` | How often the mod samples player movement. Lower values react faster but run more often. |
| `MaxSafeHorizontalSpeed` | Speed above which movement begins counting as unsafe pressure. |
| `SoftThrottleStartSpeed` | Speed where soft velocity throttling can begin. |
| `HardMaxHorizontalSpeed` | Speed above which movement is treated as exploit-level and corrected. |
| `ExtremeSpeed` | Upper bound used to scale soft throttle severity. |
| `MinThrottleFactor` | Strongest soft throttle multiplier. Lower values slow players harder. |
| `AbuseIncreasePerSecond` | How quickly sustained pressure increases abuse score. |
| `AbuseDecayPerSecond` | How quickly abuse score decays during compliant movement. |
| `PressureDirectionDotThreshold` | How straight movement must be to count as sustained directional pressure. |
| `SustainedPressureSeconds` | Time required before fast straight movement becomes pressure. |
| `GlobalPressurePlayerCount` | Number of pressured players required to trigger global slowdown. |
| `GlobalThrottleFactor` | Extra multiplier used during global pressure. |
| `ExemptPrivilege` | Privilege that exempts a player from enforcement. |
| `MaxChunkColumnsPerTick` | Maximum allowed chunk-column jump per sample before correction. |
| `CorrectionCooldownMs` | Cooldown after correction before the player can update safe position again. |
| `ExtremeViolationIncrease` | Score added for hard violations. |
| `ExtremeViolationDecay` | Score removed during compliant movement. |
| `MaxExtremeViolationScore` | Score threshold used for repeated high-speed correction. |
| `ReduceChunkRadiusOnCorrection` | Whether to reduce chunk radius after correction. |
| `CorrectiveChunkSentRadius` | Temporary chunk sent radius used after correction. |
| `WarningCooldownSeconds` | Minimum time between player warning messages. |
| `AllowTeleportGraceHeuristic` | Allows one-shot server-side teleports without engine flags. |
| `TeleportGraceCooldownMs` | Cooldown before another heuristic teleport can be accepted. |
| `TeleportArrivalGraceMs` | Status grace window after an accepted teleport. |
| `TeleportMinimumDistance` | Minimum jump distance required for the teleport heuristic. |
| `TeleportMaxRecentViolationScore` | Maximum recent violation/abuse score allowed for heuristic teleport acceptance. |

## Tuning Guide

### If hacked clients can still overflow chunks

Try these in order:

1. Lower `HardMaxHorizontalSpeed`.
2. Keep `MaxChunkColumnsPerTick` at `1`.
3. Increase `CorrectionCooldownMs`.
4. Lower `CorrectiveChunkSentRadius`.
5. Disable `AllowTeleportGraceHeuristic` if teleport abuse is suspected.

### If legitimate fast travel feels too restrictive

Try:

1. Raise `MaxSafeHorizontalSpeed`.
2. Raise `HardMaxHorizontalSpeed`.
3. Raise `ExtremeSpeed`.
4. Raise `MinThrottleFactor` closer to `1.0`.

### If legitimate Vintage Story teleports are corrected incorrectly

Try:

1. Keep `AllowTeleportGraceHeuristic` set to `true`.
2. Lower `TeleportMinimumDistance` if teleports are short.
3. Increase `TeleportGraceCooldownMs` only if repeated legitimate server-side teleports need more spacing.
4. Confirm the player was not already being corrected or flagged for speed abuse before teleporting.

## Build Requirements

To build the mod locally, you need:

- Windows, or another environment with .NET SDK support.
- .NET SDK compatible with `net10.0`.
- Vintage Story installed locally.
- `VintagestoryAPI.dll` available from the Vintage Story install.

This project defaults to:

```text
%APPDATA%\Vintagestory\VintagestoryAPI.dll
```

You can override the Vintage Story install path by setting the `VINTAGE_STORY` environment variable.

PowerShell example:

```powershell
$env:VINTAGE_STORY = "<VintageStoryInstallPath>"
dotnet build .\ChunkQueueTweaks.slnx -c Release
```

## Build Instructions

Run commands from the repository root, where `ChunkQueueTweaks.slnx` is located.

### Restore and Build Debug

```powershell
dotnet build .\ChunkQueueTweaks.slnx
```

Debug output is written to:

```text
ChunkQueueTweaks\bin\Debug\net10.0
```

### Build Release

```powershell
dotnet build .\ChunkQueueTweaks.slnx -c Release
```

Release output is written to:

```text
ChunkQueueTweaks\bin\Release\net10.0
```

The required release files are:

```text
ChunkQueueTweaks.dll
modinfo.json
```

### Create a Zip Package

Vintage Story can load code mods from a zip containing the DLL and `modinfo.json`.

PowerShell:

```powershell
$packageDir = Resolve-Path ".\ChunkQueueTweaks\bin\Release\net10.0"
$zip = Join-Path $packageDir "ChunkQueueTweaks-1.0.0.zip"

if (Test-Path -LiteralPath $zip) {
    Remove-Item -LiteralPath $zip
}

Compress-Archive `
    -Path (Join-Path $packageDir "ChunkQueueTweaks.dll"), (Join-Path $packageDir "modinfo.json") `
    -DestinationPath $zip
```

The resulting package is:

```text
ChunkQueueTweaks\bin\Release\net10.0\ChunkQueueTweaks-1.0.0.zip
```

### Verify Zip Contents

```powershell
[System.IO.Compression.ZipFile]::OpenRead(".\ChunkQueueTweaks\bin\Release\net10.0\ChunkQueueTweaks-1.0.0.zip").Entries |
    Select-Object FullName, Length
```

Expected contents:

```text
ChunkQueueTweaks.dll
modinfo.json
```

## Installation

1. Build the release zip.
2. Stop the Vintage Story server.
3. Place `ChunkQueueTweaks-1.0.0.zip` in the server `Mods` folder.
4. Start the server.
5. Run:

```text
/chunkqueuetweaks status
```

If the command responds, the mod is loaded.

## Updating

1. Stop the server.
2. Replace the old zip in the server `Mods` folder.
3. Start the server.
4. Check `/chunkqueuetweaks status`.
5. Review `chunkqueuetweaks.json` for any new config fields.

If new config fields do not appear automatically, either add them manually from this README or temporarily move the config file and let the mod regenerate defaults.

## Validation Checklist

After installing or tuning, test these scenarios:

- Normal walking is unaffected.
- Sprinting is unaffected.
- Falling and jumping are not corrected.
- Translocators work.
- Legitimate server-side teleports work.
- Admin teleports work for exempt admins.
- A hacked speed client cannot keep advancing across terrain.
- Repeated large jumps are corrected after the teleport heuristic is consumed.
- Server logs do not show chunk queue overflow during abuse testing.

## Troubleshooting

### The mod does not load

- Confirm the zip contains `ChunkQueueTweaks.dll` and `modinfo.json` at the zip root.
- Confirm the server is running a compatible Vintage Story version.
- Check server logs for missing dependency or API load errors.

### Build fails because `VintagestoryAPI.dll` is missing

Set `VINTAGE_STORY` to your Vintage Story install folder:

```powershell
$env:VINTAGE_STORY = "<VintageStoryInstallPath>"
dotnet build .\ChunkQueueTweaks.slnx -c Release
```

### Legitimate teleports are corrected

- Ensure `AllowTeleportGraceHeuristic` is `true`.
- Increase `TeleportGraceCooldownMs` only if legitimate teleports are close together.
- Lower `TeleportMinimumDistance` if legitimate server-side teleports use short hops.
- Confirm the player is not already being flagged for speed abuse before teleporting.

### Hacked movement is still too effective

- Lower `HardMaxHorizontalSpeed`.
- Keep `MaxChunkColumnsPerTick` at `1`.
- Increase `CorrectionCooldownMs`.
- Lower `CorrectiveChunkSentRadius`.
- Consider disabling `AllowTeleportGraceHeuristic` if the attacker is abusing repeated teleport-like jumps.

## Architecture Notes

The code is split into narrow classes so the tick loop stays understandable:

- Movement sampling.
- Pressure detection.
- Abuse scoring.
- Hard movement violation detection.
- Safe position tracking.
- Teleport signal detection.
- Teleport heuristic handling.
- Corrective position application.
- Chunk radius pressure reduction.
- Command formatting.

The mod deliberately avoids expensive server operations in the hot path:

- No full loaded chunk dictionary scans.
- No blocking map chunk existence checks.
- No background thread coordination.
- No client-specific detection logic.

The movement tick loop is also designed to avoid unnecessary allocation pressure:

- Reusable runtime services are kept by the tick processor instead of being recreated for each player sample.
- Disconnected-player state cleanup runs on a slower interval instead of every movement tick.
- Cleanup uses a reusable scratch list instead of LINQ-based temporary collections.
- Hard movement enforcement avoids repeated evaluator calls while preserving the same correction decisions.

## License

Apache 2.0 License