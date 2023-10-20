using Godot;
using System;

public partial class FinishRoom : Room
{
	Merchant _merchant;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		_merchant = GetNode<Merchant>("Merchant");


		if (GameState.Level > 1)
		{
			_merchant.Disable();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
