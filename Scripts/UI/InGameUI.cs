using Godot;
using System;

public partial class InGameUI : CanvasLayer
{
	

	Label _statsHealth;
	Label _statsStrength;
	Label _statsDefence;
	Label _statsMagic;
	Label _statsLuck;
	Label _statsGold;
	Label _statsKeys;

    RichTextLabel _debugPanel;
    TileMap _statsPanel;
	TileMap _inventoryPanel;

	ColorRect _healthBar;
	float _healthBarMaxWidth;

	ColorRect _manaBar;
	float _manaBarMaxWidth;

	bool _showDebug = false;
	bool _showStats = false;
	bool _showInventory = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_debugPanel = GetNode<RichTextLabel>("Debug");
		_inventoryPanel = GetNode<TileMap>("Inventory");

		_statsPanel = GetNode<TileMap>("Stats");
		_statsHealth = GetNode<Label>("Stats/Health");
        _statsStrength = GetNode<Label>("Stats/Strength");
        _statsDefence = GetNode<Label>("Stats/Defence");
        _statsMagic = GetNode<Label>("Stats/Magic");
        _statsLuck = GetNode<Label>("Stats/Luck");

        _statsGold = GetNode<Label>("Stats/Gold");
        _statsKeys = GetNode<Label>("Stats/Keys");

		_healthBar = GetNode<ColorRect>("HUD/Health");
		_healthBarMaxWidth = _healthBar.Size.X;

		_manaBar = GetNode<ColorRect>("HUD/Mana");
		_manaBarMaxWidth = _manaBar.Size.X;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ToggleDebug"))
		{
			_showDebug = !_showDebug;
		}

		if (Input.IsActionJustPressed("ToggleStats"))
		{
			_showStats = !_showStats;
			_showInventory = false;
		}

		if (Input.IsActionJustPressed("ToggleInventory"))
		{
			_showInventory = !_showInventory;
			_showStats = false;
		}

        if (_showDebug)
            _debugPanel.Show();
        else
            _debugPanel.Hide();

        if (_showStats)
            _statsPanel.Show();
        else
            _statsPanel.Hide();

		if (_showInventory)
			_inventoryPanel.Show();
		else
			_inventoryPanel.Hide();
			

        UpdateStats();
		UpdatePlayer();
	}

	private void UpdateStats()
	{
		_statsHealth.Text = Player.player.MaxHealth.ToString();
		_statsStrength.Text = Player.player.Strength.ToString();
		_statsDefence.Text = Player.player.Defence.ToString();
		_statsMagic.Text = Player.player.Magic.ToString();
		_statsLuck.Text = Player.player.Luck.ToString();

		_statsGold.Text = Player.player.Gold.ToString();
		_statsKeys.Text = Player.player.Keys.ToString();
	}

	private void UpdatePlayer()
	{
		_healthBar.SetSize(new Vector2((Player.player.Health / Player.player.MaxHealth) * _healthBarMaxWidth, _healthBar.Size.Y));
		_manaBar.SetSize(new Vector2((Player.player.Mana / Player.player.MaxMana) * _manaBarMaxWidth, _manaBar.Size.Y));
	}
}
