using Godot;
using System;

public partial class EnemySpawner : Resource
{
	[Export] public PackedScene Scene;
	[Export] public float SpawnChance;
	[Export] public float MaxSpawn;
}
