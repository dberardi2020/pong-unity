---
title: Pong — Components
date: 2026-04-21
type: reference
---

# Pong — Components

Which Unity components this game uses, what role they play here, and why they were chosen.

> Full component explanations live in the [StudioHQ component reference](../../../RoundToItStudioHQ/Docs/Notes/unity-components.md).

---

## By Object

### Main Camera
| Component | Role in this game |
|-----------|------------------|
| Camera | Renders the scene. Orthographic projection — no perspective distortion, standard for 2D. Size=5 gives a 10-unit tall play area. |

### TopWall / BottomWall
| Component | Role in this game |
|-----------|------------------|
| Sprite Renderer | Makes the wall visible as a white bar |
| Box Collider 2D | Gives the wall a solid surface for the ball to bounce off |

*No Rigidbody — walls never move, so they're treated as static surfaces.*

### PlayerPaddle / AIPaddle
| Component | Role in this game |
|-----------|------------------|
| Sprite Renderer | Makes the paddle visible as a white rectangle |
| Rigidbody 2D (Kinematic) | Puts the paddle in the physics world so the ball bounces off it. Kinematic because scripts control movement, not physics forces. |
| Box Collider 2D | Defines the paddle's collision surface |
| PlayerPaddleController / AIPaddleController | Custom script — handles movement logic |

### Ball
| Component | Role in this game |
|-----------|------------------|
| Sprite Renderer | Makes the ball visible as a white circle |
| Rigidbody 2D (Dynamic) | Physics engine owns the ball's movement. Gravity=0, Continuous collision detection, rotation frozen. |
| Circle Collider 2D | Round collision shape — more accurate than a box for a ball |
| Physics Material 2D | Friction=0, Bounciness=1 — ball never loses energy on bounce |
| BallController | Custom script — launch, speed lock, reset |

### LeftGoal / RightGoal
| Component | Role in this game |
|-----------|------------------|
| Box Collider 2D (Trigger) | Invisible zone — ball passes through and fires OnTriggerEnter2D |
| GoalZone | Custom script — tells GameManager who scored |

*No Sprite Renderer — goals are intentionally invisible.*

### Canvas
| Component | Role in this game |
|-----------|------------------|
| Canvas | Container for all UI. Screen Space - Overlay so it renders on top of the game. |
| PlayerScore / AIScore (TMP) | Displays current score for each side |

### GameManager
| Component | Role in this game |
|-----------|------------------|
| GameManager | Custom script — singleton that tracks score, detects win condition, triggers scene transitions |

### DebugOverlay
| Component | Role in this game |
|-----------|------------------|
| DebugOverlay | Custom script — toggles DebugPanel visibility and writes ball velocity/speed to DebugText each frame |

*Lives on its own always-active empty GameObject. DebugPanel starts inactive — if the script were on the panel, it could never receive input to show itself.*
