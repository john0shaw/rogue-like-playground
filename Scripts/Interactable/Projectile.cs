using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
	public ProjectileResource ProjectileResource;

	public Vector2 Origin;
	public Vector2 Target;
	public float _speed;
	public float _range;
	public float _damage;

	Vector2 _angle;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_speed = ProjectileResource.Speed;
		_range = ProjectileResource.Range;
		_damage = ProjectileResource.Damage;
		_angle = Origin.DirectionTo(Target);
		GlobalPosition = Origin;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float travelledDistance = GlobalPosition.DistanceTo(Origin);
		if (travelledDistance >= _range)
		{
			QueueFree();
			return;
		}

		Velocity = _angle * _speed;
        KinematicCollision2D collision = MoveAndCollide(
             Velocity * (float)delta
        );
        
		if (collision != null)
		{
			if (collision.GetCollider() is not EnemyController)
			{
				if (collision.GetCollider() is Player)
				{
					Player.player.TakeDamage(_damage);
				}

				QueueFree();
			}
		}
	}
}
