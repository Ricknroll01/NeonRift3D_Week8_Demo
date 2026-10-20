using Godot;
using System;

public partial class Player3D : CharacterBody3D, IDamageable
{
	[Signal]
	public delegate void ShotRequestedEventHandler(
		Vector3 position,
		Vector3 direction);

	[Signal]
	public delegate void DiedEventHandler();

	private const float MaxHealth = 100f;
	private const float MoveSpeed = 10f;
	private const float FireRate = 5f;

	private float _fireCooldown;
	private float _invulnerable;

	private float _speedMultiplier = 1f;
	private float _speedBoostTime = 0f;

	private Vector3 _aimDirection = Vector3.Forward;

	private Vector2 _mousePosition;
	private bool _hasMousePosition;

	public bool Active { get; set; } = true;

	public float Health { get; private set; } = MaxHealth;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_mousePosition = mouseMotion.Position;
			_hasMousePosition = true;
		}
		else if (@event is InputEventMouseButton mouseButton)
		{
			_mousePosition = mouseButton.Position;
			_hasMousePosition = true;
		}
	}

	public override void _PhysicsProcess(double deltaValue)
	{
		float delta = Math.Min(
			(float)deltaValue,
			0.033f);

		_fireCooldown = Math.Max(
			0f,
			_fireCooldown - delta);

		_invulnerable = Math.Max(
			0f,
			_invulnerable - delta);

		if (!Active)
		{
			Velocity = Vector3.Zero;
			return;
		}

		Vector2 input = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
			"move_down");

		Vector3 movement = new(
			input.X,
			0f,
			input.Y);

		Velocity = movement * MoveSpeed * _speedMultiplier;

		MoveAndSlide();

		AimAtMouse();

		if (Input.IsMouseButtonPressed(MouseButton.Left)
			&& _fireCooldown <= 0f)
		{
			Fire();
		}


		// Count down speed boost time if active
		if (_speedBoostTime > 0f)
		{
			_speedBoostTime -= (float)delta;

			if (_speedBoostTime <= 0f)
			{
				_speedBoostTime = 0f;
				_speedMultiplier = 1f;
				GD.Print("Speed boost expired!");
			}
		}

	}

	private void AimAtMouse()
	{
		if (!_hasMousePosition)
		{
			return;
		}

		Camera3D? camera =
			GetViewport().GetCamera3D();

		if (camera == null)
		{
			return;
		}

		Vector3 rayOrigin =
			camera.ProjectRayOrigin(_mousePosition);

		Vector3 rayDirection =
			camera.ProjectRayNormal(_mousePosition);

		if (Math.Abs(rayDirection.Y) < 0.001f)
		{
			return;
		}

		float distance =
			(GlobalPosition.Y - rayOrigin.Y)
			/ rayDirection.Y;

		if (distance <= 0f)
		{
			return;
		}

		Vector3 target =
			rayOrigin + rayDirection * distance;

		Vector3 direction =
			target - GlobalPosition;

		direction.Y = 0f;

		if (direction.LengthSquared() < 0.001f)
		{
			return;
		}

		_aimDirection =
			direction.Normalized();

		LookAt(
			GlobalPosition + _aimDirection,
			Vector3.Up);
	}

	private void Fire()
	{
		_fireCooldown = 1f / FireRate;

		EmitSignal(
			SignalName.ShotRequested,
			GlobalPosition + _aimDirection * 0.9f,
			_aimDirection);
	}

	public void TakeDamage(float damage)
	{
		if (!Active || _invulnerable > 0f)
		{
			return;
		}

		Health = Math.Max(
			0f,
			Health - damage);
		GD.Print($"Current HP: {Health}");

		_invulnerable = 0.6f;

		if (Health <= 0f)
		{
			Active = false;
			Visible = false;

			EmitSignal(
				SignalName.Died);
		}
	}


	public void Heal(float amount)
	{
		if (!Active || amount <= 0f)
			return;

		Health = Math.Min(100f, Health + amount);
		GD.Print($"Current HP: {Health}");
	}

	public void ApplySpeedBoost(float multiplier, float duration)
	{
		if (!Active || multiplier <= 0f || duration <= 0f)
			return;

		_speedMultiplier = multiplier;
		_speedBoostTime = duration;
	}



	public void ResetPlayer(Vector3 position)
	{
		GlobalPosition = position;

		Velocity = Vector3.Zero;

		Health = MaxHealth;

		_fireCooldown = 0f;
		_invulnerable = 0f;

		_aimDirection = Vector3.Forward;

		Active = true;
		Visible = true;
	}
}
