using Godot;
using System;

public partial class Game3D : Node3D
{
	private const int TargetKills = 10;

	private readonly RandomNumberGenerator _rng =
		new();

	private PackedScene _chaserScene = null!;

	private PackedScene _strikerScene = null!;

	private PackedScene _bulletScene = null!;

	private Node3D _entities = null!;

	private Player3D _player = null!;

	private float _spawnTimer;

	private int _kills;

	private int _score;

	private bool _running;

	public override void _Ready()
	{
		Engine.TimeScale = 1.0;
		
		_rng.Randomize();

		_chaserScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/ChaserEnemy3D.tscn");

		_strikerScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/StrikerEnemy3D.tscn");

		_bulletScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/Bullet3D.tscn");

		_entities =
			GetNode<Node3D>("Entities");

		_player =
			GetNode<Player3D>("Player");

		_player.ShotRequested +=
			SpawnPlayerBullet;

		_player.Died +=
			EndWithDefeat;

		StartGame();
	}

	public override void _PhysicsProcess(
		double deltaValue)
	{
		if (!_running)
		{
			return;
		}

		float delta = Math.Min(
			(float)deltaValue,
			0.033f);

		_spawnTimer -= delta;

		if (_spawnTimer <= 0f)
		{
			SpawnEnemy();

			_spawnTimer = 2.5f;
		}
	}

	private void StartGame()
	{
		ClearEntities();

		_kills = 0;

		_score = 0;

		_spawnTimer = 1.5f;

		_running = true;

		_player.ResetPlayer(
			new Vector3(0f, 0.5f, 0f));
	}

	private void SpawnEnemy()
	{
		PackedScene selectedScene =
			_kills < 4
				? _chaserScene
				: _strikerScene;

		Enemy3D enemy =
			selectedScene.Instantiate<Enemy3D>();

		_entities.AddChild(enemy);

		enemy.GlobalPosition =
			RandomArenaEdgePosition();

		enemy.Configure(_player);

		enemy.Destroyed +=
			OnEnemyDestroyed;

		enemy.ShotRequested +=
			SpawnEnemyBullet;
	}

	private Vector3 RandomArenaEdgePosition()
	{
		const float MaxX = 8.5f;
		const float MaxZ = 5.5f;

		int side =
			_rng.RandiRange(0, 3);

		if (side == 0)
		{
			return new Vector3(
				_rng.RandfRange(-MaxX, MaxX),
				0.5f,
				-MaxZ);
		}

		if (side == 1)
		{
			return new Vector3(
				MaxX,
				0.5f,
				_rng.RandfRange(-MaxZ, MaxZ));
		}

		if (side == 2)
		{
			return new Vector3(
				_rng.RandfRange(-MaxX, MaxX),
				0.5f,
				MaxZ);
		}

		return new Vector3(
			-MaxX,
			0.5f,
			_rng.RandfRange(-MaxZ, MaxZ));
	}

	private void SpawnPlayerBullet(
		Vector3 position,
		Vector3 direction)
	{
		SpawnBullet(
			position,
			direction,
			false,
			14f,
			24f);
	}

	private void SpawnEnemyBullet(
		Vector3 position,
		Vector3 direction)
	{
		SpawnBullet(
			position,
			direction,
			true,
			8f,
			11f);
	}

	private void SpawnBullet(
		Vector3 position,
		Vector3 direction,
		bool hostile,
		float speed,
		float damage)
	{
		Bullet3D bullet =
			_bulletScene.Instantiate<Bullet3D>();

		_entities.AddChild(bullet);

		bullet.GlobalPosition =
			position;

		bullet.Configure(
			direction,
			speed,
			damage,
			hostile);
	}

	private void OnEnemyDestroyed(int points)
	{
		if (!_running)
		{
			return;
		}

		_kills++;

		_score += points;

		GD.Print(
			$"Kills: {_kills}/{TargetKills}"
			+ $" | Score: {_score}");

		if (_kills >= TargetKills)
		{
			_running = false;

			_player.Active = false;

			GD.Print("VICTORY");
		}
	}

	private void EndWithDefeat()
	{
		if (!_running)
		{
			return;
		}

		_running = false;

		GD.Print("GAME OVER");
	}

	private void ClearEntities()
	{
		foreach (Node child
			in _entities.GetChildren())
		{
			child.QueueFree();
		}
	}
}
