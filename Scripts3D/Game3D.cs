using Godot;
using System;

public partial class Game3D : Node3D
{
	private const int TargetKills = 10;

	private PackedScene _bulletScene = null!;

	private Node3D _entities = null!;

	private Player3D _player = null!;

	private PowerUpSpawner3D _powerUpSpawner =
		null!;

	private EnemySpawner3D _enemySpawner =
		null!;

	private int _kills;

	private int _score;

	private bool _running;


	public override void _Ready()
	{
		Engine.TimeScale = 1.0;

		_bulletScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/Bullet3D.tscn");

		_entities =
			GetNode<Node3D>("Entities");

		_player =
			GetNode<Player3D>("Player");

		_powerUpSpawner =
			GetNode<PowerUpSpawner3D>(
				"PowerUpSpawner3D");

		_enemySpawner =
			GetNode<EnemySpawner3D>(
				"EnemySpawner3D");

		_enemySpawner.Configure(
			_player,
			_entities,
			OnEnemyDestroyed,
			SpawnEnemyBullet);

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

		_enemySpawner.UpdateSpawner(
			delta,
			_kills);
	}


	private void StartGame()
	{
		ClearEntities();

		_kills = 0;

		_score = 0;

		_running = true;

		_player.ResetPlayer(
			new Vector3(0f, 0.5f, 0f));

		_enemySpawner.StartSpawning();

		_powerUpSpawner.StartSpawning();
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


	private void OnEnemyDestroyed(
		int points)
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

			_enemySpawner.StopSpawning();

			_powerUpSpawner.StopSpawning();

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

		_enemySpawner.StopSpawning();

		_powerUpSpawner.StopSpawning();

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