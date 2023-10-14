using Godot;
using System;

public partial class DialogIndicator : Sprite2D
{
	AnimationPlayer _animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("UpDown");
	}

	public void _on_visibility_changed()
	{
		if (Visible)
			_animationPlayer?.Play("UpDown");
		else
			_animationPlayer?.Stop();
	}
}
