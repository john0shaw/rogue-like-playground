using Godot;
using System;

public partial class NPCNode : CharacterBody2D
{
	protected AnimationPlayer _idleAnimationPlayer;
    protected StateMachine _dialogStateMachine;
    protected InGameUI _inGameUI;
    protected InteractableComponent _interactable;
    protected DialogIndicator _dialogIndicator;

	CollisionShape2D _collosionShape2D;

	public override void _Ready()
	{
		_inGameUI = InGameUI.inGameUI;
		_idleAnimationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
		_dialogStateMachine = GetNode<StateMachine>("DialogStateMachine");

		_interactable = GetNodeOrNull<InteractableComponent>("InteractableComponent");
		_dialogIndicator = GetNodeOrNull<DialogIndicator>("DialogIndicator");

		_collosionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		_idleAnimationPlayer.Play("Idle");
	}

	public void Disable()
	{
		Hide();
		_collosionShape2D.Disabled = true;
	}

	public void Enable()
	{
		Show();
		_collosionShape2D.Disabled = false;
	}

	public virtual void Interact()
	{

	}

	public Dialog SayCurrentDialog()
	{
		NPCDialogState state = (NPCDialogState)_dialogStateMachine.State;
        return _inGameUI.SayDialog(state.DialogResources.PickRandom());
    }

    public void _on_interactable_component_interacted()
    {
		Interact();
    }

    public void _on_interactable_component_entered_range()
	{
		_dialogIndicator?.Hide();
	}

	public void _on_interactable_component_exited_range()
	{
		_dialogIndicator?.Show();
	}
}
