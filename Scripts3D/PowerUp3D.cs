
using Godot;

public abstract partial class PowerUp3D : Area3D
{
    private bool _collected = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_collected || body is not Player3D player)
            return;

        if (!player.Active)
            return;

        _collected = true;

        ApplyPowerUp(player);

        QueueFree();
    }

    protected abstract void ApplyPowerUp(Player3D player);
}
