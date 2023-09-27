using Godot;
using System;

public partial class InGameUI : CanvasLayer
{
	private Label _health;
	private Label _gold;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = GetNode<Label>("HUD/Health");
		_gold = GetNode<Label>("HUD/Gold");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_health.Text = "Health: " + Player.player.Health;
		_gold.Text = "Gold: " + Player.player.GetItemCountByID(0);
	}
}
