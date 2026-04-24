# Pong — Design Document

## Overview

A single-player 2D Pong game built in Unity 6. The player controls a paddle on the left side; an AI controls the right paddle. First to a target score wins.

No custom art assets — everything is built with Unity primitives (quads/sprites) and the built-in UI system.

---

## Gameplay

| Element | Description |
|--------|-------------|
| Player | Controls left paddle via keyboard (W/S or Arrow Up/Down) |
| AI | Controls right paddle, tracks ball with configurable difficulty |
| Ball | Launches at game start, increases speed on each paddle hit |
| Scoring | Point scored when ball passes a paddle; displayed on screen |
| Win condition | First to reach target score (e.g. 5 points) |

---

## Scenes

| Scene | Purpose |
|-------|---------|
| `MainMenu` | Title, Play button, Quit button |
| `Game` | Core gameplay |
| `GameOver` | Win/loss result, Play Again, Main Menu buttons |

Unity loads scenes by name — we'll use `SceneManager.LoadScene()` to transition between them.

## Scene Structure — Game

```
Scene: Game
├── Camera (Orthographic, 2D)
├── Ball
├── PlayerPaddle (left)
├── AIPaddle (right)
├── Walls
│   ├── TopWall
│   └── BottomWall
├── Goals (invisible triggers)
│   ├── LeftGoal   ← AI scores here
│   └── RightGoal  ← Player scores here
├── GameManager
├── DebugOverlay
└── UI (Canvas)
    ├── PlayerScore
    ├── AIScore
    └── DebugPanel (inactive by default)
        └── DebugText
```

## Scene Structure — MainMenu

```
Scene: MainMenu
└── UI (Canvas)
    ├── Title ("PONG")
    ├── PlayButton → loads Game scene
    └── QuitButton → Application.Quit()
```

## Scene Structure — GameOver

```
Scene: GameOver
└── UI (Canvas)
    ├── ResultText ("You Win!" / "You Lose!")
    ├── PlayAgainButton → loads Game scene
    └── MainMenuButton → loads MainMenu scene
```

---

## Scripts

| Script | Responsibility |
|--------|---------------|
| `BallController` | Launch, bounce speed lock, per-hit speed scaling, reset on score |
| `PlayerPaddleController` | Read keyboard input, move paddle within bounds |
| `AIPaddleController` | Track ball Y position, move toward it with a speed cap (difficulty) |
| `GameManager` | Track score, detect win condition, trigger scene transitions |
| `GoalZone` | Detect ball entering a goal trigger, notify GameManager |
| `MainMenuController` | Wire up Play and Quit buttons |
| `GameOverController` | Display result text, wire up Play Again and Main Menu buttons |
| `DebugOverlay` | Toggle an in-game debug panel showing ball velocity and speed |

---

## Technical Choices

- **2D physics** — Rigidbody2D + Collider2D for ball and paddles. Keeps things simple and Unity-native.
- **Orthographic camera** — Standard for 2D games; no perspective distortion. Camera size 5 = 10 unit tall play area.
- **Physics Material 2D** — Zero friction, full bounciness on ball so it doesn't lose energy on bounce.
- **FixedUpdate for physics** — Movement that interacts with physics runs in FixedUpdate, not Update, for consistency.
- **No assets** — Unity primitive shapes (white quads) for all game objects; TextMeshPro for score UI.
- **uGUI for UI** — Canvas-based UI (com.unity.ugui). Not included by default in Unity 6 — must be added to manifest. See StudioHQ `unity-ui-systems.md`.
- **Kinematic paddles** — Paddles use Kinematic Rigidbody2D, moved via `MovePosition`. Bound calculated from wall position, wall half-height, paddle half-height, and a small padding gap — same formula on both paddles.
- **Velocity normalization** — Ball velocity is normalized and re-applied every FixedUpdate to prevent speed drift from bounces.
- **Speed scaling** — Ball speed increments on each paddle hit up to a max. `baseSpeed` captured at Start so reset restores the Inspector value, not a hardcoded constant.
- **Debug overlay on always-active object** — `DebugOverlay` lives on its own empty GameObject, not on `DebugPanel`. A panel that starts inactive won't run scripts — it couldn't receive input to show itself.

---

## Scene Layout

| Object | Position | Scale |
|--------|----------|-------|
| Main Camera | (0, 0, -10) | - |
| TopWall | (0, 4.5, 0) | (20, 0.5, 1) |
| BottomWall | (0, -4.5, 0) | (20, 0.5, 1) |
| PlayerPaddle | (-8, 0, 0) | (0.5, 2, 1) |
| AIPaddle | (8, 0, 0) | (0.5, 2, 1) |
| Ball | (0, 0, 0) | (0.5, 0.5, 1) |
| LeftGoal | (-10.5, 0, 0) | (1, 10, 1) |
| RightGoal | (10.5, 0, 0) | (1, 10, 1) |

---

## Milestones

- [x] Game scene setup — camera, walls, paddles, ball as primitives
- [x] Ball movement and bouncing
- [x] Player input
- [x] AI paddle
- [x] Debug overlay
- [x] Scoring and goal detection
- [ ] Win condition and scene transition to GameOver
- [ ] MainMenu scene
- [ ] GameOver scene
- [ ] Polish (AI tuning, background color)
