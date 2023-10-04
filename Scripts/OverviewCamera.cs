using Godot;
using System;

public partial class OverviewCamera : CharacterBody2D
{
	[Export] public float Speed = 1000.0f;

	public override void _PhysicsProcess(double delta)
	{
        Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
        if (direction != Vector2.Zero)
		{
			Velocity = direction * Speed;
		}
		else
		{
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();
	}
}
