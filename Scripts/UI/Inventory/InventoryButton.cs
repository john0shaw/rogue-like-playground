using Godot;
using System;

public partial class InventoryButton : TextureButton
{
	Item _item;
	Sprite2D _sprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		Disabled = true;
	}

	public void SetItem(Item item)
	{
		_item = item;
		_sprite2D.Texture = item.Texture;
		if (item is Weapon)
			Disabled = false;
	}

	public void _on_mouse_entered()
	{
		Player.player.TrackMouseEvents = false;
	}

	public void _on_mouse_exited()
	{
		Player.player.TrackMouseEvents = true;
	}

	public void _on_pressed()
	{
		if (_item is Weapon)
		{
			Player.player.SetWeapon((Weapon)_item);
		}
	}
}
