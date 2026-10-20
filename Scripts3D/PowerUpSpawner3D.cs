
using Godot;

public partial class PowerUpSpawner3D : Node3D
{
	private PackedScene _healPowerUpScene;
	private PackedScene _speedPowerUpScene;

	public float SpawnInterval = 5f;
	public float SpawnRange = 5.5f;

	private float _spawnTimer = 0f;
	private bool _running = false;

	private readonly RandomNumberGenerator _random = new();

	public override void _Ready()
	{
		_healPowerUpScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/HealPowerUp3D.tscn");

		_speedPowerUpScene =
			GD.Load<PackedScene>(
				"res://Scenes3D/SpeedPowerUp3D.tscn");
	}

	public void StartSpawning()
	{
		_spawnTimer = 0f;
		_running = true;
	}

	public void StopSpawning()
	{
		_running = false;
	}

	public override void _Process(double delta)
	{
		if (!_running || SpawnInterval <= 0f)
			return;

		_spawnTimer += (float)delta;

		if (_spawnTimer >= SpawnInterval)
		{
			_spawnTimer = 0f;
			SpawnPowerUp();
		}
	}

	private void SpawnPowerUp()
	{
		// Randomly choose between Heal and Speed.
		PackedScene selectedScene =
			_random.RandiRange(0, 1) == 0
				? _healPowerUpScene
				: _speedPowerUpScene;

		if (selectedScene == null)
			return;

		PowerUp3D powerUp = selectedScene.Instantiate<PowerUp3D>();

		// Add the power-up to the the entities node.
		GetNode<Node3D>("../Entities").AddChild(powerUp);

		// Random position on the floor.
		float x = _random.RandfRange(-SpawnRange, SpawnRange);
		float z = _random.RandfRange(-SpawnRange, SpawnRange);

		powerUp.GlobalPosition =
			GlobalPosition + new Vector3(x, 0.5f, z);

		GD.Print($"Spawned {powerUp.GetType().Name}");
	}
}
