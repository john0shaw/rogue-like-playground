using Godot;
using System;

public partial class Dialog : ColorRect
{
    [Signal] public delegate void DialogFinishedEventHandler();
	[Signal] public delegate void ConfirmClickedEventHandler();
	[Signal] public delegate void CancelClickedEventHandler();
    
	[Export] public DialogResource DialogResource;
	[Export] public AudioStream ButtonHover;
	[Export] public AudioStream ButtonClick;
	[Export] public float TextSpeed = 0.05f;
	[Export] public AudioStream CursorSound;
	[Export] public AudioStream NextSound;

	public AudioStreamPlayer UIAudioPlayer;

	Timer _timer;
	RichTextLabel _name;
	RichTextLabel _text;
	Sprite2D _indicator;
	AnimationPlayer _indicatorAnimationPlayer;
	HBoxContainer _buttonsContainer;

	int _phraseNum = 0;
	bool _finished = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_name = GetNode<RichTextLabel>("Name");
		_text = GetNode<RichTextLabel>("Text");
		_indicator = GetNode<Sprite2D>("Indicator");
		_indicatorAnimationPlayer = GetNode<AnimationPlayer>("Indicator/AnimationPlayer");
		_buttonsContainer = GetNode<HBoxContainer>("Buttons");

		_timer.WaitTime = TextSpeed;
		GameState.DialogOpen = true;

		_buttonsContainer.Hide();

		NextPhrase();
	}

	private async void NextPhrase()
	{
		_indicator.Hide();
		_indicatorAnimationPlayer.Stop();

		if (_phraseNum >= DialogResource.Dialog.Count)
		{
			GameState.DialogOpen = false;
			EmitSignal("DialogFinished");
			if (DialogResource.Confirm)
			{
				_buttonsContainer.Show();
			}
			else
			{
                QueueFree();
            }
			return;
		}

		_finished = false;
		_name.Text = DialogResource.Speaker;
		_text.Text = DialogResource.Dialog[_phraseNum];
		PlayUIAudio(CursorSound);

		_text.VisibleCharacters = 0;

		while (_text.VisibleCharacters < _text.Text.Length)
		{
			_text.VisibleCharacters++;

			_timer.Start();
			await ToSignal(_timer, "timeout"); 
		}

		StopUIAudio();
        _indicator.Show();
        _indicatorAnimationPlayer.Play("Bounce");
        _finished = true;
        _phraseNum++;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			if (_finished)
			{
				if (!(DialogResource.Confirm && _buttonsContainer.Visible))
				{
                    PlayUIAudio(NextSound);
                    NextPhrase();
                }
            }
			else if (_text.VisibleCharacters > 3)
				_text.VisibleCharacters = _text.Text.Length;

		}
	}

	void StopUIAudio()
	{
		UIAudioPlayer?.Stop();
	}

	void PlayUIAudio(AudioStream stream)
	{
		if (UIAudioPlayer is AudioStreamPlayer)
		{
			UIAudioPlayer.Stream = stream;
			UIAudioPlayer.Play();
		}
	}

	public void _on_confirm_pressed()
	{
		PlayUIAudio(ButtonClick);
		EmitSignal("ConfirmClicked");
		QueueFree();
	}

	public void _on_confirm_mouse_entered()
	{
		PlayUIAudio(ButtonHover);
	}

	public void _on_cancel_pressed()
	{
		PlayUIAudio(ButtonClick);
		EmitSignal("CancelClicked");
		QueueFree();
	}

	public void _on_cancel_mouse_entered()
	{
		PlayUIAudio(ButtonHover);
	}
}
