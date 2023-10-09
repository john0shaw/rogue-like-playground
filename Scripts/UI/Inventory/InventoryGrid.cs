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
			_inventoryButtons.Add(_button);
			AddChild(_button);
		}
	}

	public void _on_player_inventory_updated()
	{
		int positionTrack = 0;
		foreach (Item item in Player.player.Inventory)
		{
			_inventoryButtons[positionTrack].SetItem(item);
			positionTrack++;
		}
	}
}
