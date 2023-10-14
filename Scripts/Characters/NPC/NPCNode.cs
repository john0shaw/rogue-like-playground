using Godot;
using System;

public partial class NPCNode : CharacterBody2D
{
	AnimationPlayer _idleAnimationPlayer;
	StateMachine _dialogStateMachine;
	InGameUI _inGameUI;

	public override void _Ready()
	{
		_inGameUI = InGameUI.inGameUI;
		_idleAnimationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
		_dialogStateMachine = GetNode<StateMachine>("DialogStateMachine");

		_idleAnimationPlayer.Play("Idle");
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	public void _on_interactable_component_interacted()
	{
		SayDialog();
	}

	public void SayDialog()
	{
		NPCDialogState state = (NPCDialogState)_dialogStateMachine.State;

        _inGameUI.SayDialog(state.DialogResources.PickRandom());

        switch (state.Name)
        {
            case "FirstGreetings":
				// say first greetings
				_dialogStateMachine.TransitionTo("RandomChatter");
                break;
        }
    }
}
