using Godot;
using System;

public partial class InventoryButton : TextureButton
{
	public enum ModeEnum
	{
		Inventory,
		Shop
	};

	public int InventoryIndex;
	public ModeEnum Mode;
	

	public Item Item;
	Label _label;
	Sprite2D _sprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_label = GetNode<Label>("Label");

		_sprite2D.Show();
		_label.Hide();
		
		Disabled = true;
	}

	public void SetItem(Item item)
	{
		Item = item;
		_sprite2D.Texture = item.Texture;
		_label.Text = item.Name;
		Disabled = false;
	}

	public void Clear()
	{
		Item = null;
		_label.Text = "";
		_sprite2D.Texture = null;
		Disabled = true;
	}

	public void _on_mouse_entered()
	{
		Player.player.TrackMouseEvents = false;
		_sprite2D.Hide();
		_label.Show();
	}

	public void _on_mouse_exited()
	{
		Player.player.TrackMouseEvents = true;
		_sprite2D.Show();
		_label.Hide();
	}

	public void _on_pressed()
	{
		if (Mode == ModeEnum.Inventory)
		{
            if (Item is Weapon)
            {
                Player.player.SetWeapon((Weapon)Item);
            }
            else if (Item is Consumable)
            {
                if (Item is Potion)
                    Player.player.DrinkPotion((Potion)Item);
                else
                    Logger.Log("Unknown Consumable used - " + Item.Name);

                Player.player.RemoveItemAt(InventoryIndex);
            }
        }
		else if (Mode == ModeEnum.Shop)
		{
			if (Item != Player.player.EquipedWeapon)
			{
                Player.player.SellItem(Item);
                Player.player.RemoveItemAt(InventoryIndex);
            }
		}
	}
}
