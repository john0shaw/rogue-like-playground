using Godot;
using System;

public partial class InGameUI : CanvasLayer
{
	public static InGameUI inGameUI;

	PackedScene _dialogScene;

	Label _statsHealth;
	Label _statsStrength;
	Label _statsDefence;
	Label _statsGold;
	Label _statsLevel;

    RichTextLabel _debugPanel;
    TileMap _statsPanel;
	TileMap _inventoryPanel;
	InventoryGrid _inventoryGrid;
	Label _inventoryGold;
	HUD _hud;
	AudioStreamPlayer _audioStreamPlayer;

	ColorRect _healthBar;
	float _healthBarMaxWidth;

	bool _showDebug = false;
	bool _showStats = false;
	bool _showInventory = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		inGameUI = this;

		_dialogScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Dialog.tscn");

		_debugPanel = GetNode<RichTextLabel>("Debug");
		_inventoryPanel = GetNode<TileMap>("Inventory");
		_inventoryGrid = GetNode<InventoryGrid>("Inventory/InventoryGrid");
		_inventoryGold = GetNode<Label>("Inventory/Gold");
		_hud = GetNode<HUD>("HUD");
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		_statsPanel = GetNode<TileMap>("Stats");
		_statsHealth = GetNode<Label>("Stats/Health");
        _statsStrength = GetNode<Label>("Stats/Strength");
        _statsDefence = GetNode<Label>("Stats/Defence");
		_statsLevel = GetNode<Label>("Stats/Level");

        _statsGold = GetNode<Label>("Stats/Gold");

		_healthBar = GetNode<ColorRect>("HUD/Health");
		_healthBarMaxWidth = _healthBar.Size.X;
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

		if (GameState.DialogOpen)
		{
			_showStats = false;
			_showInventory = false;
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

		_statsGold.Text = Player.player.Gold.ToString();
		_statsLevel.Text = GameState.Level.ToString();

		_inventoryGold.Text = Player.player.Gold.ToString();
	}

	private void UpdatePlayer()
	{
		_healthBar.SetSize(new Vector2((Player.player.Health / Player.player.MaxHealth) * _healthBarMaxWidth, _healthBar.Size.Y));
	}

	public Dialog SayDialog(DialogResource dialogResource)
	{
		Dialog dialog = (Dialog)_dialogScene.Instantiate();
		dialog.DialogResource = dialogResource;
		dialog.UIAudioPlayer = _audioStreamPlayer;
		AddChild(dialog);

		return dialog;
	}

	public void _on_player_changed_weapon()
	{
		_hud.SetWeapon(Player.player.EquipedWeapon);
	}

	public void _on_player_inventory_updated()
	{
		_inventoryGrid.Update();
	}
}
