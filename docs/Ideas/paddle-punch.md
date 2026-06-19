---
title: Paddle Punch
date: 2026-04-24
type: idea
status: in-progress
branch: experiment/paddle-punch
---

# Paddle Punch

Active player ability — press A or D to rotate the paddle like a pinball flipper, redirecting the ball at a disruptive angle with a speed boost.

## Mechanic

- **A** — top of paddle swings toward ball (clockwise rotation)
- **D** — bottom of paddle swings toward ball (counter-clockwise)
- Paddle quickly rotates to peak angle, then snaps back
- Cooldown prevents spamming
- Ball hit during the outward swing gets a speed boost on top of the normal per-hit increment
- Angle disruption is natural — rotated collider changes deflection angle through physics

## Implementation

- `PaddlePunch.cs` on `PlayerPaddle` — handles input, coroutine, cooldown, `IsPunching` flag
- `BallController.cs` — checks `IsPunching` in `OnCollisionEnter2D`, applies `punchSpeedBoost`
- No scene restructuring needed — rotates the existing single paddle GameObject

## Tuning (starting values)

| Parameter | Value |
|-----------|-------|
| Punch angle | 30° |
| Punch duration | 0.08s |
| Snap duration | 0.1s |
| Cooldown | 0.5s |
| Speed boost | +3f |

## Open Questions

- Does the cooldown feel right, or does it make the mechanic frustrating?
- Should the AI respond differently to punched returns?
- Could this become a limited resource (stamina bar) rather than a flat cooldown?
