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

	public void SetWeapon(Weapon weapon)
	{
		_activeWeaponSprite.Texture = weapon.Texture;
	}
}
