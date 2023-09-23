using Godot;
using System;

public partial class WeaponNode : Node2D
{
	private RigidBody2D _colliderOrigin;
	private Sprite2D _sprite2D;
	private Node2D _spriteOrigin;
	private CollisionShape2D _collider;
	private AnimationPlayer _animationPlayer;

	private Weapon _weapon;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_colliderOrigin = GetNode<RigidBody2D>("ColliderOrigin");
		_sprite2D = GetNode<Sprite2D>("SpriteOrigin/Sprite2D");
		_collider = GetNode<CollisionShape2D>("ColliderOrigin/CollisionShape2D");
		_spriteOrigin = GetNode<Node2D>("SpriteOrigin");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_collider.Disabled = true;
	}

	public void SetWeapon(Weapon weapon)
	{
		_weapon = weapon;
		_sprite2D.Texture = weapon.Texture;
	}

	public void RotateWeapon(float radians)
	{
		_colliderOrigin.Rotation = radians;
		_spriteOrigin.Rotation = radians;
	}

	public void Attack()
	{
		if (_weapon.AttackType == Weapon.AttackTypeEnum.Stab)
		{
			_animationPlayer.Play("Stab");
        }
		else if (_weapon.AttackType == Weapon.AttackTypeEnum.Swing)
		{
			_animationPlayer.Play("Swing");
		}
		
	}
}
