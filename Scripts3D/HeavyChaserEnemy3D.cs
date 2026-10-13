using Godot;

public partial class HeavyChaserEnemy3D : ChaserEnemy3D
{
	protected override void ConfigureStats()
	{
		MaxHealth = 100f;

		MoveSpeed = 1.2f;
	}
}
