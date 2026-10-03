using Godot;
using System;

public abstract partial class Enemy3D
	: CharacterBody3D
{
	[Signal]
	public delegate void DestroyedEventHandler(
		int points);

	[Signal]
	public delegate void ShotRequestedEventHandler(
		Vector3 position,
		Vector3 direction);

	protected Player3D Target { get; private set; }
		= null!;

	protected float MoveSpeed { get; set; }

	protected float ContactDamage { get; set; }

	protected int Points { get; set; }

	protected float MaxHealth { get; set; }

	public float Health { get; private set; }

	private bool _dead;

	private float _contactCooldown;

	public void Configure(Player3D target)
	{
		Target = target;

		ConfigureStats();

		Health = MaxHealth;
	}

	protected abstract void ConfigureStats();

	protected abstract void UpdateBehaviour(
		float delta,
		float distance,
		Vector3 direction);

	public override void _PhysicsProcess(
		double deltaValue)
	{
		if (_dead
			|| !IsInstanceValid(Target)
			|| !Target.Active)
		{
			Velocity = Vector3.Zero;
			return;
		}

		float delta = Math.Min(
			(float)deltaValue,
			0.033f);

		_contactCooldown = Math.Max(
			0f,
			_contactCooldown - delta);

		Vector3 toPlayer =
			Target.GlobalPosition - GlobalPosition;

		toPlayer.Y = 0f;

		float distance = Math.Max(
			toPlayer.Length(),
			0.001f);

		Vector3 direction =
			toPlayer / distance;

		LookAt(
			GlobalPosition + direction,
			Vector3.Up);

		UpdateBehaviour(
			delta,
			distance,
			direction);

		MoveAndSlide();

		CheckPlayerContact(distance);
	}

	public void TakeDamage(float damage)
	{
		if (_dead)
		{
			return;
		}

		Health = Math.Max(
			0f,
			Health - damage);

		if (Health <= 0f)
		{
			Die();
		}
	}

	protected void RequestShot(
		Vector3 direction)
	{
		EmitSignal(
			SignalName.ShotRequested,
			GlobalPosition + direction * 1.0f,
			direction);
	}

	private void CheckPlayerContact(float distance)
	{
		const float ContactDistance = 1.1f;

		if (distance <= ContactDistance
			&& _contactCooldown <= 0f)
		{
			Target.TakeDamage(ContactDamage);

			_contactCooldown = 0.6f;
		}
	}

	private void Die()
	{
		_dead = true;

		EmitSignal(
			SignalName.Destroyed,
			Points);

		QueueFree();
	}
}
