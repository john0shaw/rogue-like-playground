using Godot;
using Godot.Collections;
using System;

public partial class EnemyIdleWanderState : EnemyState
{
	[Export] float _detectionDistance = 100f;
	[Export] float _waitTimer = 2f;
	[Export] int _wanderRange = 50;

	float _waitTime = 0f;
	RandomNumberGenerator _rng;
	Vector2 _wanderTarget;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

    public override void Initialize()
    {
		base.Initialize();
		_wanderTarget = _enemyController.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
		float distanceToPlayer = _enemyController.GlobalPosition.DistanceTo(Player.player.GlobalPosition);
		if(distanceToPlayer < _enemyController.EnemyResource.DetectionDistance)
		{
            StateMachine.TransitionTo("Chasing");
			return;
        }

		if (_waitTime >= 0f)
		{
			_enemyController.PlayAnimation("Idle");
			_waitTime -= (float)delta;
			return;
		}

		_enemyController.PlayAnimation("Walk");


		if (_enemyController.GlobalPosition.DistanceTo(_wanderTarget) < 1f)
			ResetWander();
		else
			Wander(delta);
    }

	private void Wander(double delta)
	{
		_enemyController.Velocity = _enemyController.GlobalPosition.DirectionTo(_wanderTarget) * _enemyController.EnemyResource.MoveSpeed;
        KinematicCollision2D collision = _enemyController.MoveAndCollide(
			 _enemyController.Velocity * (float)delta
		);

		if (collision != null)
			ResetWander();
    }

	private void ResetWander()
	{
        _enemyController.Velocity = Vector2.Zero;
        _wanderTarget = new Vector2(
            _enemyController.GlobalPosition.X + _rng.RandiRange(-_wanderRange, _wanderRange),
            _enemyController.GlobalPosition.Y + _rng.RandiRange(-_wanderRange, _wanderRange)
        );

        _waitTime = _waitTimer;
    }

    public override void Enter(Dictionary message = null)
    {
		Logger.Log("IdleEnter");
    }

    public override void Exit()
    {
        Logger.Log("IdleExit");
    }
}
