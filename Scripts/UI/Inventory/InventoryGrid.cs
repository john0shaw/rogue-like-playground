using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryGrid : HFlowContainer
{
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

			positionTrack++;
		}
	}
}
