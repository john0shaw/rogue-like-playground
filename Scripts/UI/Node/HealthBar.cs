using Godot;
using System;

public partial class HealthBar : Node2D
{
	public float MaxHealth;
	public float Health;

	ColorRect _backgroundBar;
    ColorRect _healthBar;
	float _maxWidth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backgroundBar = GetNode<ColorRect>("BackgroundBar");
		_healthBar = GetNode<ColorRect>("BackgroundBar/Health");

		_maxWidth = _backgroundBar.Size.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (MaxHealth == 0)
			return;

		_healthBar.SetSize(new Vector2((Health / MaxHealth) * _maxWidth, _healthBar.Size.Y));
	}
}
