# AGENT.md — ChunkQueueTweaks

## 🧭 Mission

Implement a **server-side stability system** that prevents chunk-loading queue overflow by controlling player-induced streaming pressure using a **hybrid throttling model**:

1. Soft velocity throttling (primary control)
2. Progressive escalation (abuse-based)
3. Chunk-pressure awareness (backpressure signal)

The system must **reduce chunk request generation rate** without teleporting players.

---

## ⚠️ Constraints

* Do NOT modify engine internals
* Do NOT teleport or rubberband players
* Do NOT block server threads
* Must be lightweight (runs every tick)
* Must not impact normal gameplay

---

## 🧩 Core Problem

Server crash occurs when:

* Players move faster than chunk generation throughput
* Chunk requests accumulate faster than they are processed
* Queue exceeds capacity → overflow → crash

System responsibility:

> Ensure no player can sustain a chunk request rate that exceeds server processing capacity.

---

## 🏗️ System Architecture

### 1. Movement Monitor

Tracks per-player:

* Position delta
* Speed (units/sec)
* Movement consistency (directional vs random)

Outputs:

* Current speed
* Sustained velocity signal

---

### 2. Throttle Controller (Primary Layer)

Applies **soft movement throttling** when:

* Speed exceeds `MaxSafeSpeed`
* OR sustained high-speed travel detected

Behavior:

* Gradual velocity reduction (not abrupt)
* Proportional to violation severity

Goal:

* Reduce forward chunk demand rate

---

### 3. Abuse Tracker (Escalation Layer)

Maintains per-player score:

* Increases when exceeding safe streaming limits
* Decays over time when compliant

Score tiers drive throttle intensity:

* Low → minimal/no effect
* Medium → moderate slowdown
* High → strong slowdown

Goal:

* Prevent repeated pressure spikes

---

### 4. Chunk Pressure Heuristic (Backpressure Signal)

Since direct queue access is unavailable, infer pressure using:

Signals:

* Sustained high velocity
* Linear travel over distance
* Repeated threshold violations

Interpretation:

* Player is outrunning chunk generation

Effect:

* Accelerate escalation
* Increase throttle strength earlier

---

### 5. Global Failsafe

Monitors aggregate behavior:

* Multiple players exceeding limits simultaneously

Response:

* Apply mild global slowdown factor
* Prevent cascading queue overflow

---

## 🔁 Tick Loop Responsibilities

Each tick:

1. Update player movement data
2. Compute speed + directionality
3. Update abuse score
4. Evaluate chunk pressure heuristics
5. Apply throttle adjustments
6. Apply escalation effects if needed

---

## ⚙️ Control Strategy (Hybrid Model)

### Base Rule

If player speed > safe streaming speed:
→ apply proportional slowdown

### Reinforcement Rule

If violations persist:
→ increase abuse score
→ strengthen slowdown

### Backpressure Rule

If sustained directional high-speed travel:
→ treat as chunk pressure event
→ escalate faster

---

## 🎯 Success Criteria

* Chunk request rate stays within safe bounds
* No queue overflow under high-speed movement
* No teleportation occurs
* Legitimate movement unaffected
* System reacts smoothly, not abruptly

---

## 🚫 Anti-Patterns

Avoid:

* Hard position correction
* Instant full stops (unless extreme abuse)
* Static speed caps without context
* Ignoring sustained movement patterns

---

## 🧪 Validation Scenarios

1. Normal play → no slowdown
2. Sprinting → no noticeable impact
3. Moderate flight → slight regulation
4. Extreme speed exploit → heavy throttling
5. Multiple fast players → system remains stable

---

## 🧠 Key Insight

This is a **rate-limiting system**, not a punishment system.

Control:

> Player velocity → chunk request rate → queue stability

---

## 📦 Deliverables

* Modular systems (monitor, throttle, tracker, pressure)
* Configurable thresholds
* Server-safe performance

---

## 🔄 Execution Order

1. Movement monitoring
2. Base throttle logic
3. Abuse tracking
4. Pressure heuristics
5. Global failsafe
6. Tuning + validation

---
