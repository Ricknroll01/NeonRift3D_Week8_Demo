# Program Structure and Call Flow

## Top-level flow

```text
Godot loads Scenes/Main.tscn
    -> Game._Ready()
        -> load reusable Enemy and Bullet scenes
        -> connect Player signals
        -> show title screen

Input event
    -> Game._Input()
        -> StartGame() or Player.TryDash()

Every physics frame
    -> Game._PhysicsProcess()
        -> SpawnEnemy() when the timer expires
        -> Hud.UpdateStatus()
    -> Player._PhysicsProcess()
        -> Input.GetVector()
        -> CharacterBody2D.MoveAndSlide()
        -> emit ShotRequested when firing
    -> Enemy._PhysicsProcess()
        -> CharacterBody2D.MoveAndSlide()
        -> check contact collision
        -> emit ShotRequested for Striker attacks
    -> Bullet._PhysicsProcess()
        -> CharacterBody2D.MoveAndCollide()
        -> Player.TakeDamage() or Enemy.TakeDamage()
```

## Signal relationships

```text
Player.ShotRequested  -> Game.SpawnPlayerBullet()
Player.Died           -> Game.EndWithDefeat()

Enemy.ShotRequested   -> Game.SpawnEnemyBullet()
Enemy.Destroyed       -> Game.OnEnemyDestroyed()

Game                  -> Hud.ShowTitle()/ShowPlaying()/ShowVictory()/ShowGameOver()
Game                  -> Hud.UpdateStatus()
```

## Collision layers

| Layer | Object |
| ---: | --- |
| 1 | Player |
| 2 | Enemies |
| 4 | Projectiles |
| 8 | Arena walls |

The collision masks decide which layers each moving body can detect. The
colliders themselves are configured in `.tscn` scene files rather than being
constructed in `Game.cs`.
