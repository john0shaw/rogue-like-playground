using Godot;
using System;

public partial class ExitDoor : StaticBody2D
{
	AnimationPlayer _animationPlayer;
	InteractableComponent _interactable;
	AnimationPlayer _fadeAnimationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_interactable = GetNode<InteractableComponent>("InteractableComponent");
		_fadeAnimationPlayer = GetNode<AnimationPlayer>("Fade/AnimationPlayer");
		_animationPlayer = GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer");
		_animationPlayer.Play("Glow");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void _on_interactable_component_interacted()
	{
		GameState.Level++;
		GameState.Save();
		_fadeAnimationPlayer.Play("Fade");
		await ToSignal(_fadeAnimationPlayer, "animation_finished");
		GetTree().ReloadCurrentScene();
	}
}
