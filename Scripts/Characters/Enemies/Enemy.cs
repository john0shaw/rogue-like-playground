using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 100f;
	[Export] public int StartingHealth = 1;
    [Export] public Array<Item> Loot = new Array<Item>();

	public int Health;

	private RandomNumberGenerator _rng;
	private Sprite2D _attackIndicator;

	private bool _playerInAttackRange = false;
	private bool _lockAnimation = false;
	private string _lockedAnimationName;

	enum State
	{
		Idle,
		Chasing,
		Attacking
	}

	State _state;
	Vector2 _moveTarget;

	AnimationPlayer _animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();
		_rng.Randomize();

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_attackIndicator = GetNode<Sprite2D>("AttackIndicator");

		_moveTarget = GlobalPosition;
        _state = State.Idle;
        Health = StartingHealth;
    }

    public void TakeDamage(int Damage)
	{
		Health -= Damage;
	}

	public void Die()
	{
		SpawnLoot();
		QueueFree();
	}

	public void SpawnLoot()
	{
		Node2D roomNode = GetParent<Node2D>();

		foreach (Item loot in Loot)
		{
            for (int i = 0; i < loot.SpawnMax; i++)
            {
                PackedScene packedScene = (PackedScene)ResourceLoader.Load(loot.ScenePath);
                if (_rng.Randf() < loot.SpawnChance)
                {
                    Node2D lootNode = (Node2D)packedScene.Instantiate();
                    roomNode.AddChild(lootNode);
                    lootNode.Position = new Vector2(Position.X + _rng.RandiRange(-10, 10), Position.Y + _rng.RandiRange(-10, 10));
                }
            }
        }
	}

	public void FinishAttack()
	{
		Player.player.TakeDamage(1);
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
		if (Health <= 0)
		{
			_animationPlayer.Play("Die");
			return;
		}

		switch(_state)
		{
			case State.Chasing:
				_moveTarget = Player.player.GlobalPosition;
				break;
			case State.Attacking:
                _moveTarget = Player.player.GlobalPosition;
				break;
			default:
            case State.Idle:
				SetRandomWanderTarget();
                break;
        }

        MoveTowardsTarget(delta, Speed);
		Animate();
    }

	private void Animate()
	{
		if (_state == State.Attacking)
		{
			_animationPlayer.Play("Attack");
			_attackIndicator.GlobalPosition = Player.player.GlobalPosition;
		}
		else if (Velocity != Vector2.Zero)
		{
			if (Velocity.X > 0)
			{
				_animationPlayer.Play("Walk");
			}	
			else
			{
				_animationPlayer.PlayBackwards("Walk");
			}
		}
		else
		{
			_animationPlayer.Play("Idle");	
		}
	}

	private void PlayAnimation(string animation, bool playForward = true, bool lockAnimation = false)
	{
		string animationName = animation;

		if (_lockAnimation)
			animationName = _lockedAnimationName;

		if (!_lockAnimation && lockAnimation)
		{
			_lockedAnimationName = animation;
			_lockAnimation = true;
		}

		if (playForward)
			_animationPlayer.Play(animation);
		else
			_animationPlayer.PlayBackwards(animation);
	}

	private void SetRandomWanderTarget()
	{
		int r = _rng.RandiRange(0, 100);
		if (r > 99)
		{
			Vector2 newPatrolTarget = GlobalPosition;
			newPatrolTarget.X += _rng.RandiRange(-50, 50);
			newPatrolTarget.Y += _rng.RandiRange(-50, 50);
            _moveTarget = newPatrolTarget;
		}
	}

	private void MoveTowardsTarget(double delta, float speed)
	{
		Vector2 targetPosition = _moveTarget;
		Vector2 direction = GlobalPosition.DirectionTo(targetPosition);

		if (_moveTarget == GlobalPosition || GlobalPosition.DistanceTo(_moveTarget) < 1f)
		{
			Velocity = Vector2.Zero;
			_moveTarget = GlobalPosition;
		}	
		else
		{
            Velocity = direction * speed;
            MoveAndSlide();
        }
	}

	public void _on_hearing_origin_body_entered(Node2D node)
	{
		if (node is Player)
		{
            _moveTarget = Player.player.GlobalPosition;
			_state = State.Chasing;
		}
	}

	public void _on_hearing_origin_body_exited(Node2D node)
	{
		if (node is Player)
		{
            _moveTarget = GlobalPosition;
            _state = State.Idle;
		}
	}

	public void _on_attacking_origin_body_entered(Node2D node)
	{
		if (node is Player)
		{
			_playerInAttackRange = true;
			_state = State.Attacking;
		}
	}

	public void _on_attacking_origin_body_exited(Node2D node)
	{
		if (node is Player)
		{
			_playerInAttackRange = false;
			_attackIndicator.Hide();
			_state = State.Chasing;
		}
	}

	public void _on_animation_player_animation_finished(string animation)
	{
		if (_lockAnimation)
			_lockAnimation = false;

		if (animation == "Attack" && _playerInAttackRange)
		{
			GD.Print("Attacked Player!");
		}
	}
}
