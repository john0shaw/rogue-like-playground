using Godot;
using System;
using Godot.Collections;

public partial class EnemyChasingState : EnemyState
{
    public override void _PhysicsProcess(double delta)
    {
		float distanceToPlayer = _enemyController.GlobalPosition.DistanceTo(Player.player.GlobalPosition);
		if(distanceToPlayer > _enemyController.EnemyResource.DetectionDistance)
		{
            StateMachine.TransitionTo("Idle");
			return;
        }
			
		if (distanceToPlayer <= _enemyController.EnemyResource.AttackRange)
		{
            StateMachine.TransitionTo("Attack");
			return;
		}
		else
		{
            _enemyController.Velocity = _enemyController.GlobalPosition.DirectionTo(Player.player.GlobalPosition) * _enemyController.EnemyResource.MoveSpeed;
        }


        _enemyController.PlayAnimation("Walk");
        _enemyController.MoveAndSlide();
    }

	public override void Enter(Dictionary message = null)
	{
        Logger.Log("ChasingEnter");
	}

    public override void Exit()
    {
        Logger.Log("ChasingExit");
        _enemyController.PlayAnimation("RESET");
    }
}
