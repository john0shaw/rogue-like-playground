using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public const float Speed = 100.0f;

	private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			_animationPlayer.Play("Walk");
		}
		else
		{
			velocity = Vector2.Zero;
			_animationPlayer.Play("Idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
