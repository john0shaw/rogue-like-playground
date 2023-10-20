using Godot;
using System;

public partial class Shop : CanvasLayer
{
	private Label _gold;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gold = GetNode<Label>("TileMap/Gold");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_gold.Text = Player.player.Gold.ToString();
	}
}
