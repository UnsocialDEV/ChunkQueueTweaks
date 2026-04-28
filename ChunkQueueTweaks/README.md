# ChunkQueueTweaks

## Overview

**ChunkQueueTweaks** is a server-side stability mod designed to prevent crashes caused by excessive chunk loading.

It protects the server from the classic:

> *“Indexed Fifo Queue overflow”*

by ensuring players cannot generate chunk requests faster than the server can handle.

---

## 🧠 How It Works

The mod uses a **hybrid throttling system** that controls how quickly players can move relative to chunk generation.

Instead of teleporting or punishing players, it:

* Gradually slows movement when it becomes unsafe
* Detects sustained high-speed travel
* Applies increasing resistance if abuse continues

This keeps gameplay smooth while protecting server stability.

---

## ⚙️ Core Mechanisms

### 1. Movement Throttling

When a player exceeds safe movement speed:

* Their velocity is gently reduced
* No teleporting or rubberbanding

---

### 2. Progressive Slowdown

If a player continues to push limits:

* A hidden score increases
* Movement becomes more restricted over time

---

### 3. Chunk Pressure Detection

The mod identifies when a player is:

* Moving fast in a straight line
* Continuously entering unloaded terrain

This indicates chunk streaming stress, and the system responds faster.

---

### 4. Global Protection

If multiple players stress the system at once:

* A mild global slowdown may apply
* Prevents total server overload

---

## 🎯 What This Prevents

* Chunk loading queue overflow
* Server crashes from excessive terrain generation
* Exploits involving extreme flight speed

---

## ✅ What This Does NOT Do

* Does NOT teleport players
* Does NOT kick players (by default)
* Does NOT modify game files or clients

---

## ⚙️ Configuration (Conceptual)

Server admins can tune:

* Maximum safe movement speed
* How aggressively slowdown is applied
* How quickly repeated violations escalate

---

## 🧪 Expected Behavior

| Scenario              | Result                   |
| --------------------- | ------------------------ |
| Normal gameplay       | No effect                |
| Sprinting             | No effect                |
| Fast travel (legit)   | Slight soft cap          |
| Extreme speed exploit | Strong slowdown          |
| Multiple fast players | System stabilizes server |

---

## 💡 Why This Works

Chunk loading is a **rate-limited system**.

Crashes occur when:

* Chunk requests are created faster than processed

This mod fixes the root cause by:

> Controlling the rate at which players can generate new chunk requests

---

## 🛠️ Installation

1. Place the mod in your server’s mods folder
2. Restart the server
3. No client installation required

---

## 🧠 Summary

ChunkQueueTweaks ensures:

* Stable chunk streaming
* Smooth gameplay
* No more queue overflow crashes

All without disrupting the player experience.

---
