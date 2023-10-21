using Godot;
using System;

public partial class Training : CanvasLayer
{
	private Label _gold;

	private Label _statsHealthLabel;
	private Label _statsStrengthLabel;
	private Label _statsDefenceLabel;

	private Label _costHealthLabel;
	private Label _costStrengthLabel;
	private Label _costDefenceLabel;

	private Button _upgradeHealthButton;
	private Button _upgradeStrengthButton;
	private Button _upgradeDefenceButton;

	private int _costHealth;
	private int _costStrength;
	private int _costDefence;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gold = GetNode<Label>("TileMap/Gold");

		_statsHealthLabel = GetNode<Label>("TileMap/Stats/Health");
		_statsStrengthLabel = GetNode<Label>("TileMap/Stats/Strength");
		_statsDefenceLabel = GetNode<Label>("TileMap/Stats/Defence");

		_costHealthLabel = GetNode<Label>("TileMap/Cost/Health");
		_costStrengthLabel = GetNode<Label>("TileMap/Cost/Strength");
		_costDefenceLabel = GetNode<Label>("TileMap/Cost/Defence");

		_upgradeHealthButton = GetNode<Button>("TileMap/Upgrade/Health");
		_upgradeHealthButton.Pressed += _on_upgrade_health_pressed;
		_upgradeStrengthButton = GetNode<Button>("TileMap/Upgrade/Strength");
		_upgradeStrengthButton.Pressed += _on_upgrade_strength_pressed;
		_upgradeDefenceButton = GetNode<Button>("TileMap/Upgrade/Defence");
		_upgradeDefenceButton.Pressed += _on_upgrade_defence_pressed;

		Update();
	}

	public void Update()
	{
		_statsHealthLabel.Text = Player.player.MaxHealth.ToString();
		_statsStrengthLabel.Text = Player.player.Strength.ToString();
		_statsDefenceLabel.Text = Player.player.Defence.ToString();

		_costHealth = Mathf.FloorToInt(Mathf.Pow(Player.player.MaxHealth, 1 + (float)GameState.Level / 10));
		_costStrength = Mathf.FloorToInt(Mathf.Pow(Player.player.Strength, 1 + (float)GameState.Level / 2));
		_costDefence = Mathf.FloorToInt(Mathf.Pow(Player.player.Defence, 1 + (float)GameState.Level / 3));

		_costHealthLabel.Text = _costHealth.ToString();
		_costStrengthLabel.Text = _costStrength.ToString();
		_costDefenceLabel.Text = _costDefence.ToString();

		_upgradeHealthButton.Disabled = Player.player.Gold < _costHealth;
		_upgradeStrengthButton.Disabled = Player.player.Gold < _costStrength;
		_upgradeDefenceButton.Disabled = Player.player.Gold < _costDefence;

		_gold.Text = Player.player.Gold.ToString();
	}

	public void _on_upgrade_health_pressed()
	{
		Player.player.UpgradeHealth(_costHealth);
		Update();
	}

	public void _on_upgrade_strength_pressed()
	{
		Player.player.UpgradeStrength(_costStrength);
		Update();
	}

	public void _on_upgrade_defence_pressed()
	{
		Player.player.UpgradeDefence(_costDefence);
		Update();
	}
}
