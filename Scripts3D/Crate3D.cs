using Godot;

public partial class Crate3D : StaticBody3D, IDamageable
{
	public float MaxHealth = 50f;

	private float _health;

	public override void _Ready()
	{
		_health = MaxHealth;
	}

	public void TakeDamage(float damage)
	{
		if (damage <= 0 || _health <= 0)
			return;

		_health -= damage;

		if (_health <= 0)
		{
			Break();
		}
	}

	private void Break()
	{
		QueueFree();
	}
}
