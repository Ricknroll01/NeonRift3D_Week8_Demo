# Neon Rift 3D — Week 8 Collaboration Demo

A simple Godot 4 C# project used for the Week 8
GitHub and group collaboration exercise.

## Running the Project

1. Open the project using Godot 4 .NET.
2. Allow the C# project to build.
3. Run the main 3D scene.

## Project Structure

- `Scenes3D/` — 3D Godot scenes
- `Scripts3D/` — C# gameplay scripts
- `Player3D.cs` — player movement, health and shooting
- `Enemy3D.cs` — base enemy class
- `ChaserEnemy3D.cs` — Chaser behaviour
- `StrikerEnemy3D.cs` — Striker behaviour
- `Game3D.cs` — main game logic

## Week 8 Collaboration Example

### Student A — Enemy Feature

Main file:

`Scripts3D/StrikerEnemy3D.cs`

Suggested task:

Change the Striker shooting interval from `1.6f` to `1.2f`.

### Student B — Player Feature

Main file:

`Scripts3D/Player3D.cs`

Suggested task:

Change player movement speed from `7f` to `8.5f`.

## Merge Conflict Example

During the collaboration demonstration, both students may
intentionally modify the following line differently:

Game objective: Defeat 10 enemies.

This will be used to demonstrate a simple merge conflict.
