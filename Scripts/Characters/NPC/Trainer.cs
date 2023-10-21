using Godot;
using Godot.Collections;
using System;

public partial class Trainer : NPCNode
{
    Training _trainingLayer;
    bool _trainingOpen = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

        _trainingLayer = GetNode<Training>("Training");
        _trainingLayer.Hide();

        SetupDialog();
	}

    public override void Interact()
    {
		Dialog dialog = SayCurrentDialog();
        NPCDialogState state = (NPCDialogState)_dialogStateMachine.State;

        if (GameState.Level > 1)
            dialog.DialogFinished += OpenShop;

		_dialogStateMachine.TransitionTo("RandomChatter");
    }

	void SetupDialog()
	{
        if (GameState.Level == 2)
        {
            _dialogStateMachine.TransitionTo("Rescue");
        }
        else if (GameState.Level == 3)
        {
            _dialogStateMachine.TransitionTo("BackAtBar");
        }
        else
        {
            _dialogStateMachine.TransitionTo("RandomChatter");
        }
    }

    public void OpenShop()
    {
        _trainingOpen = true;
        _trainingLayer.Update();
        _trainingLayer.Show();
        GetTree().Paused = true;
    }

    public void CloseShop()
    {
        _trainingOpen = false;
        _trainingLayer.Hide();
        GetTree().Paused = false;
    }

    public void _on_close_button_pressed()
    {
        CloseShop();
    }
}
