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
| Ball | Launches at game start, increases speed over time |
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
└── UI (Canvas)
    ├── PlayerScore
    └── AIScore
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
| `BallController` | Movement, bouncing off walls/paddles, speed scaling, reset on score |
| `PlayerPaddleController` | Read keyboard input, move paddle within bounds |
| `AIPaddleController` | Track ball Y position, move toward it with a speed cap (difficulty) |
| `GameManager` | Track score, detect win condition, trigger scene transitions |
| `MainMenuController` | Wire up Play and Quit buttons |
| `GameOverController` | Display result text, wire up Play Again and Main Menu buttons |

---

## Technical Choices

- **2D physics** — Rigidbody2D + Collider2D for ball and paddles. Keeps things simple and Unity-native.
- **Orthographic camera** — Standard for 2D games; no perspective distortion.
- **Physics Material 2D** — Zero friction, full bounciness on ball so it doesn't lose energy on bounce.
- **FixedUpdate for physics** — Movement that interacts with physics runs in FixedUpdate, not Update, for consistency.
- **No assets** — Unity primitive shapes (white quads) for all game objects; TextMeshPro for score UI.

---

## Milestones

- [ ] Game scene setup — camera, walls, paddles, ball as primitives
- [ ] Ball movement and bouncing
- [ ] Player input
- [ ] AI paddle
- [ ] Scoring and goal detection
- [ ] Win condition and scene transition to GameOver
- [ ] MainMenu scene
- [ ] GameOver scene
- [ ] Polish (ball speed scaling, AI tuning)
