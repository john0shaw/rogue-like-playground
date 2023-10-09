using Godot;
using Godot.Collections;
using System;

public partial class State : Node
{
    public StateMachine StateMachine;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public new virtual void _Process(double delta)
    {
        return;
    }

    public new virtual void _PhysicsProcess(double delta)
    {
        return;
    }

    public virtual void Initialize()
    {
        return;
    }

    public virtual void Enter(Dictionary message = null)
    {
        return;
    }

    public virtual void Exit()
    {
        return;
    }
}
