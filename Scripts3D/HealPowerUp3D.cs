using Godot;

public partial class HealPowerUp3D : PowerUp3D
{
    public float HealAmount = 25f;

    protected override void ApplyPowerUp(Player3D player)
    {
        player.Heal(HealAmount);

        GD.Print($"Heal PowerUp collected! +{HealAmount} HP");
        GD.Print($"Current HP: {player.Health}");
    }
}
