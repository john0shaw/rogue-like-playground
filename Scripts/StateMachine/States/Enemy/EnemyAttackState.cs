using Godot;
using System;
using Godot.Collections;

public partial class EnemyAttackState : EnemyState
{
    float _attackSpeed;
    float _attackDuration;
    float _waitTime = 0f;

    public override void Initialize()
    {
        base.Initialize();
        _attackSpeed = _enemyController.EnemyResource.AttackSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        float distanceToPlayer = _enemyController.GlobalPosition.DistanceTo(Player.player.GlobalPosition);
        if (distanceToPlayer > _enemyController.EnemyResource.AttackRange)
        {
            StateMachine.TransitionTo("Chasing");
            return;
        }

        if (_waitTime <= 0f)
        {
            Player.player.TakeDamage(_enemyController.EnemyResource.AttackDamage);
            _waitTime = _attackSpeed;
            return;
        }

        if (_waitTime <= _enemyController.GetAnimationLength(EnemyController.ATTACK_ANIMATION))
        {
            _enemyController.Velocity = Vector2.Zero;
            _enemyController.PlayAnimation("Attack");
        }
        else if (_enemyController.GlobalPosition.DistanceTo(Player.player.GlobalPosition) < 1f)
        {
            _enemyController.Velocity = Vector2.Zero;
            _enemyController.PlayAnimation("Idle");
        }
        else
        {
            _enemyController.Velocity = _enemyController.GlobalPosition.DirectionTo(Player.player.GlobalPosition) * _enemyController.EnemyResource.MoveSpeed;
            _enemyController.PlayAnimation("Walk");
        }

        _waitTime -= (float)delta;
        _enemyController.MoveAndSlide();
    }

    public override void Enter(Dictionary message = null)
    {
        Logger.Log("AttackEnter");
        _waitTime = _attackSpeed;
    }

    public override void Exit()
    {
        _enemyController.PlayAnimation("RESET");
    }
}
