using Godot;
using System;

public partial class MainMenuUI : CanvasLayer
{
	[Export] public AudioStream ButtonMouseOver;
	[Export] public AudioStream ButtonClick;

	AudioStreamPlayer _audioStreamPlayer;
	Button _continueButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_continueButton = GetNode<Button>("Buttons/Continue");
	}

	private void PlayButtonMouseOverEffect()
	{
		_audioStreamPlayer.Stream = ButtonMouseOver;
		_audioStreamPlayer.Play();
	}

	public void PlayButtonMouseClickEffect()
	{
		_audioStreamPlayer.Stream = ButtonClick;
		_audioStreamPlayer.Play();
	}

	public void _on_new_game_mouse_entered() => PlayButtonMouseOverEffect();
	public void _on_continue_mouse_entered()
	{
		if (!_continueButton.Disabled)
		{
			PlayButtonMouseOverEffect();
		}
	}
	public void _on_exit_mouse_entered() => PlayButtonMouseOverEffect();

}
