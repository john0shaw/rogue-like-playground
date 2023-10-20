using Godot;
using System;

public partial class Bartender : NPCNode
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		Setup();
	}

    public override void Interact()
    {
		SayCurrentDialog();
		_dialogStateMachine.TransitionTo("RandomChatter");
    }

	private void Setup()
	{
        if (GameState.Level == 1)
        {
            _dialogStateMachine.TransitionTo("FirstGreetings");
        }
        else if (GameState.Level == 2)
        {
            _dialogStateMachine.TransitionTo("SecondGreetings");
        }
        else
        {
            _dialogStateMachine.TransitionTo("RandomChatter");
        }
    }
}
