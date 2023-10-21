using Godot;
using System;

public partial class FinishRoom : Room
{
	Merchant _merchant;
	Trainer _trainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		_merchant = GetNode<Merchant>("Merchant");
		_trainer = GetNode<Trainer>("Trainer");

		_merchant.Disable();
		_trainer.Disable();

		if (GameState.Level == 1)
		{
			_merchant.Enable();
		}
		if (GameState.Level == 2)
		{
			_trainer.Enable();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
