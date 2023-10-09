using Godot;
using System;

public partial class InGameUI : CanvasLayer
{
	RichTextLabel _debugPane;

	Label _statsHealth;
	Label _statsStrength;
	Label _statsDefence;
	Label _statsMagic;
	Label _statsLuck;
	Label _statsGold;
	Label _statsKeys;

	bool _showDebug = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_debugPane = GetNode<RichTextLabel>("Debug");

		_statsHealth = GetNode<Label>("Stats/Health");
        _statsStrength = GetNode<Label>("Stats/Strength");
        _statsDefence = GetNode<Label>("Stats/Defence");
        _statsMagic = GetNode<Label>("Stats/Magic");
        _statsLuck = GetNode<Label>("Stats/Luck");

        _statsGold = GetNode<Label>("Stats/Gold");
        _statsKeys = GetNode<Label>("Stats/Keys");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ToggleDebug"))
		{
			_showDebug = !_showDebug;
			if (_showDebug)
				_debugPane.Show();
			else
				_debugPane.Hide();
		}

		UpdateStats();
	}

	private void UpdateStats()
	{
		_statsHealth.Text = Player.player.Health.ToString();
		_statsStrength.Text = Player.player.Strength.ToString();
		_statsDefence.Text = Player.player.Defence.ToString();
		_statsMagic.Text = Player.player.Magic.ToString();
		_statsLuck.Text = Player.player.Luck.ToString();

		_statsGold.Text = Player.player.Gold.ToString();
		_statsKeys.Text = Player.player.Keys.ToString();
	}
}
