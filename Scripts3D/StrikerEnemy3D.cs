using Godot;

public partial class StrikerEnemy3D : Enemy3D
{
	private float _shootTimer = 1f;

	protected override void ConfigureStats()
	{
		MaxHealth = 72f;

		MoveSpeed = 3.2f;

		ContactDamage = 22f;

		Points = 180;
	}

	protected override void UpdateBehaviour(
		float delta,
		float distance,
		Vector3 direction)
	{
		const float MinimumDistance = 4.5f;
		const float MaximumDistance = 6.5f;

		if (distance > MaximumDistance)
		{
			Velocity =
				direction * MoveSpeed;
		}
		else if (distance < MinimumDistance)
		{
			Velocity =
				-direction * MoveSpeed;
		}
		else
		{
			Velocity =
				Vector3.Zero;
		}

		_shootTimer -= delta;

		if (_shootTimer <= 0f
			&& distance < 12f)
		{
			RequestShot(direction);

			_shootTimer = 1.6f;
		}
	}
}
