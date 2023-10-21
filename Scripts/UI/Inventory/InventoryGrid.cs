using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryGrid : HFlowContainer
{
	[Signal] public delegate void button_pressedEventHandler();

	[Export] public InventoryButton.ModeEnum Mode;

	List<InventoryButton> _inventoryButtons = new List<InventoryButton>();
	PackedScene _inventoryItemButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _inventoryItemButton = ResourceLoader.Load<PackedScene>("res://Scenes/UI/InventoryItemButton.tscn");

		for (int i = 0; i < Player.INVENTORY_SIZE; i++)
		{
			InventoryButton _button = (InventoryButton)_inventoryItemButton.Instantiate();
			_button.InventoryIndex = i;
			_button.Mode = Mode;
			_button.Pressed += Update;
			_button.Pressed += _on_inventory_button_pressed;
			_inventoryButtons.Add(_button);
			AddChild(_button);
		}
	}

	public void Update()
	{
		int positionTrack = 0;
		foreach (InventoryButton _button in _inventoryButtons)
		{
			if (positionTrack < Player.player.Inventory.Count)
				_button.SetItem(Player.player.Inventory[positionTrack]);
			else
				_button.Clear();

			if (_button.Item == Player.player.EquipedWeapon)
				_button.Disabled = true;
			else
				_button.Disabled = false;

			positionTrack++;
		}
	}

	public void _on_inventory_button_pressed()
	{
		EmitSignal("button_pressed");
	}
}
