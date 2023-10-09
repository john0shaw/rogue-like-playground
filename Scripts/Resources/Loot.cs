using Godot;
using System;

public partial class Loot : Resource
{
	[Export] public Item Item;
	[Export] public float SpawnChance;
	[Export] public int MinSpawn;
	[Export] public int MaxSpawn;
}
