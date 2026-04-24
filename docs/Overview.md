---
title: Pong
date: 2026-03-14
updated: 2026-04-21
type: overview
status: in-progress
---

# Pong — Overview

A 2D single-player Pong clone. First Unity project — primary goal is learning Unity fundamentals, not shipping a novel game.

---

## Concept

Classic Pong: two paddles, one ball, score to win. You control the left paddle via keyboard; an AI controls the right. The ball bounces off the top and bottom walls and accelerates slightly over time. First to the target score wins.

No custom art assets — everything is built from Unity primitives and the built-in UI system.

---

## Learning Goals

- Understand Unity's component system (GameObjects, MonoBehaviours, lifecycle methods)
- Get comfortable with 2D physics, input handling, and scene management
- Work with scene transitions and simple game state
- Establish a working dev workflow with Unity

---

## Scope

### v1 — In Scope
- Single player vs AI
- Ball with physics-based bouncing and speed scaling
- Player paddle controlled via keyboard (W/S or arrow keys)
- AI paddle tracking the ball with configurable difficulty
- Score tracking with on-screen display
- Win condition (first to target score)
- Three scenes: MainMenu, Game, GameOver

### Out of Scope (v1)
- Sound and music
- Visual polish, animations, particle effects
- Local multiplayer
- Difficulty selection screen
- Persistent high scores

---

## Stack

- Unity 6.3 LTS (6000.3.14f1)
- C# scripts under `Assets/Scripts/`
- TextMeshPro for UI text
- Project: `Round To It Studio/Pong Games/pong-game-unity/`

---

## Status

See milestone checklist in [DESIGN.md](Design%20Guide.md).
