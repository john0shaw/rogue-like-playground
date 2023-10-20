using Godot;
using Godot.Collections;
using System;

public partial class GameStateResource : Resource
{
    [ExportGroup("DungeonState")]
    [Export] public int Level = 1;

    [ExportGroup("PlayerState")]
    [Export] public int Gold = 0;
    [Export] public Array<Item> Inventory = new Array<Item>();
    [Export] public Weapon EquippedWeapon;
    [Export] public float MaxHealth = 10;
    [Export] public float Health = 10;
    [Export] public int Strength = 1;
    [Export] public int Defence = 1;
}
