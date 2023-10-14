using Godot;
using System;

public partial class Dialog : ColorRect
{
	[Export] public DialogResource DialogResource;
	[Export] public float TextSpeed = 0.05f;

	Timer _timer;
	RichTextLabel _name;
	RichTextLabel _text;
	Sprite2D _indicator;
	AnimationPlayer _indicatorAnimationPlayer;

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

		_timer.WaitTime = TextSpeed;
		GameState.DialogOpen = true;

		NextPhrase();
	}

	private async void NextPhrase()
	{
		_indicator.Hide();
		_indicatorAnimationPlayer.Stop();

		if (_phraseNum >= DialogResource.Dialog.Count)
		{
			GameState.DialogOpen = false;
			QueueFree();
			return;
		}

		_finished = false;
		_name.Text = DialogResource.Speaker;
		_text.Text = DialogResource.Dialog[_phraseNum];

		_text.VisibleCharacters = 0;

		while (_text.VisibleCharacters < _text.Text.Length)
		{
			_text.VisibleCharacters++;

			_timer.Start();
			await ToSignal(_timer, "timeout"); 
		}

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
				NextPhrase();
			else if (_text.VisibleCharacters > 3)
				_text.VisibleCharacters = _text.Text.Length;

		}
	}
}
