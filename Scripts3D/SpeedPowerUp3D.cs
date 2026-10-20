
using Godot;

public partial class SpeedPowerUp3D : PowerUp3D
{
	public float SpeedMultiplier = 1.5f;
	public float Duration = 5f;

	protected override void ApplyPowerUp(Player3D player)
	{
		player.ApplySpeedBoost(SpeedMultiplier, Duration);

		GD.Print($"Speed PowerUp collected!");
		GD.Print($"Speed increased x{SpeedMultiplier} for {Duration}s");
	}
}
