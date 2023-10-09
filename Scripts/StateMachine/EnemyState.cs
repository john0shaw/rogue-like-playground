using Godot;
using Godot.Collections;
using System;

public partial class EnemyState : State
{
    protected EnemyController _enemyController;

    public override void Initialize()
    {
        _enemyController = (EnemyController)StateMachine.RootNode;
    }
}
