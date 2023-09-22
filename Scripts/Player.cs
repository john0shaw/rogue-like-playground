using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public const float Speed = 100.0f;
	[Export] public Weapon StartingWeapon;

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite2D;
	private Sprite2D _swordSprite;

	private Weapon _equippedWeapon;
	private bool _isAttacking;

    public override void _Ready()
    {
        base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_swordSprite = GetNode<Sprite2D>("Sprite2D/SwordSprite2D");

		SetWeapon(StartingWeapon);
    }

	public void SetWeapon(Weapon weapon)
	{
		_swordSprite.Texture = weapon.Texture;
		_equippedWeapon = weapon;
	}

	public void Attack()
	{
		_isAttacking = true;
		_animationPlayer.Play("Attack");
		GD.Print("Attack!");
	}

	public void FinishAttack()
	{
		_isAttacking = false;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
        Vector2 mousePosition = GetGlobalMousePosition();
		float radsToRotate = Mathf.Atan2(mousePosition.Y - Position.Y, mousePosition.X - Position.X);

		_swordSprite.Rotation = 1.571f + radsToRotate;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;

			if (!_isAttacking && direction.X > 0)
				_animationPlayer.Play("Walk");
			else
				_animationPlayer.PlayBackwards("Walk");
		}
		else
		{
			velocity = Vector2.Zero;
			if (!_isAttacking)
				_animationPlayer.Play("Idle");
		}

		if (Input.IsActionJustPressed("Attack"))
		{
			Attack();
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
