using Godot;
using System;

public partial class HUD : TileMap
{
	Sprite2D _activeWeaponSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_activeWeaponSprite = GetNode<Sprite2D>("ActiveWeapon");
	}

	public void _on_player_changed_weapon()
	{
		_activeWeaponSprite.Texture = Player.player.EquipedWeapon.Texture;
	}
}
