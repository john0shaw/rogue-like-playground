using Godot;
using System;
using Godot.Collections;

public partial class EnemyDieState : EnemyState
{
    float _animationRemainingLength;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_animationRemainingLength > 0f)
        {
            if (_enemyController.Velocity != Vector2.Zero)
            {
                _enemyController.Velocity = Vector2.Zero;
                _enemyController.MoveAndSlide();
            }
            
            _animationRemainingLength -= (float)delta;
            _enemyController.PlayAnimation(EnemyController.DIE_ANIMATION);
        }
        else
        {
            _enemyController.GenerateLoot();
            _enemyController.QueueFree();
        }
    }

    public override void Enter(Dictionary message = null)
    {
        Logger.Log("DieEnter");
        _animationRemainingLength = _enemyController.GetAnimationLength(EnemyController.DIE_ANIMATION);
    }

    public override void Exit()
    {
        _enemyController.PlayAnimation("RESET");
    }
}
