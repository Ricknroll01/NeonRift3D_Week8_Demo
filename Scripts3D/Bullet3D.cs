using Godot;
using System;

public partial class Bullet3D
	: CharacterBody3D
{
	private const uint PlayerLayer = 1u;
	private const uint EnemyLayer = 2u;
	private const uint WorldLayer = 8u;

	private bool _hostile;

	private float _damage;

	private float _life;

	public void Configure(
		Vector3 direction,
		float speed,
		float damage,
		bool hostile)
	{
		_hostile = hostile;

		_damage = damage;

		_life = hostile
			? 4f
			: 2f;

		Velocity =
			direction.Normalized() * speed;

		CollisionMask =
			WorldLayer
			| (hostile
				? PlayerLayer
				: EnemyLayer);
	}

	public override void _PhysicsProcess(
		double deltaValue)
	{
		float delta = Math.Min(
			(float)deltaValue,
			0.033f);

		_life -= delta;

		if (_life <= 0f)
		{
			QueueFree();
			return;
		}

		KinematicCollision3D? collision =
			MoveAndCollide(
				Velocity * delta);

		if (collision == null)
		{
			return;
		}

		GodotObject collider =
			collision.GetCollider();

		if (_hostile
			&& collider is Player3D player)
		{
			player.TakeDamage(_damage);
		}
		else if (!_hostile
			&& collider is Enemy3D enemy)
		{
			enemy.TakeDamage(_damage);
		}

		QueueFree();
	}
}
