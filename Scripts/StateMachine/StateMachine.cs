using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class StateMachine : Node
{
	[Signal] public delegate void TransitionedEventHandler(string stateName, Dictionary message = null);

	[Export] protected NodePath InitialState;

	public State State;
	public Node RootNode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Transitioned += TransitionTo;

		RootNode = GetOwner<Node>();

		State = GetNode<State>(InitialState);
		foreach(State childState in GetChildren())
		{
            childState.StateMachine = this;
			childState.Initialize();
		}
		State.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		State._Process(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
		State._PhysicsProcess(delta);
    }

	public void TransitionTo(string toState, Dictionary message = null)
	{
		if (!HasNode(toState))
		{
			Logger.Log(Name + ": No state matching " + toState);
            return;
        }
			
		State.Exit();
		State = GetNode<State>(toState);
		State.Enter(message);
		EmitSignal("Transitioned", toState);
	}
}
