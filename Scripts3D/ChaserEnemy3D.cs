using Godot;

public partial class ChaserEnemy3D : Enemy3D
{
	protected override void ConfigureStats()
	{
		MaxHealth = 38f;

		MoveSpeed = 2.0f;

		ContactDamage = 14f;

		Points = 100;
	}

	protected override void UpdateBehaviour(
		float delta,
		float distance,
		Vector3 direction)
	{
		const float StopDistance = 1.0f;

		if (distance > StopDistance)
		{
			Velocity =
				direction * MoveSpeed;
		}
		else
		{
			Velocity = Vector3.Zero;
		}
	}
}
