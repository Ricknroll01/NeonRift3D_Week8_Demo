using Godot;

public partial class EnemySpawner3D : Node
{
    private readonly RandomNumberGenerator _rng =
        new();

    private PackedScene _chaserScene = null!;

    private PackedScene _heavyChaserScene = null!;

    private PackedScene _strikerScene = null!;

    private Node3D _entities = null!;

    private Player3D _player = null!;

    private Enemy3D.DestroyedEventHandler
        _onEnemyDestroyed = null!;

    private Enemy3D.ShotRequestedEventHandler
        _onEnemyShotRequested = null!;

    private float _spawnTimer;

    private bool _spawning;


    public override void _Ready()
    {
        _rng.Randomize();

        _chaserScene =
            GD.Load<PackedScene>(
                "res://Scenes3D/ChaserEnemy3D.tscn");

        _heavyChaserScene =
            GD.Load<PackedScene>(
                "res://Scenes3D/HeavyChaserEnemy3D.tscn");

        _strikerScene =
            GD.Load<PackedScene>(
                "res://Scenes3D/StrikerEnemy3D.tscn");
    }


    public void Configure(
        Player3D player,
        Node3D entities,
        Enemy3D.DestroyedEventHandler onEnemyDestroyed,
        Enemy3D.ShotRequestedEventHandler onEnemyShotRequested)
    {
        _player = player;

        _entities = entities;

        _onEnemyDestroyed =
            onEnemyDestroyed;

        _onEnemyShotRequested =
            onEnemyShotRequested;
    }


    public void StartSpawning()
    {
        _spawnTimer = 1.5f;

        _spawning = true;
    }


    public void StopSpawning()
    {
        _spawning = false;
    }


    public void UpdateSpawner(
        float delta,
        int kills)
    {
        if (!_spawning)
        {
            return;
        }

        _spawnTimer -= delta;

        if (_spawnTimer <= 0f)
        {
            SpawnEnemy(kills);

            _spawnTimer = 2.5f;
        }
    }


    private void SpawnEnemy(int kills)
    {
        PackedScene selectedScene;

        if (kills < 3)
        {
            selectedScene =
                _chaserScene;
        }
        else if (kills < 6)
        {
            selectedScene =
                _heavyChaserScene;
        }
        else
        {
            selectedScene =
                _strikerScene;
        }

        Enemy3D enemy =
            selectedScene.Instantiate<Enemy3D>();

        _entities.AddChild(enemy);

        enemy.GlobalPosition =
            RandomArenaEdgePosition();

        enemy.Configure(_player);

        enemy.Destroyed +=
            _onEnemyDestroyed;

        enemy.ShotRequested +=
            _onEnemyShotRequested;
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
}