using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class EnemySpawner : Node
{
	[Export] public EnemyResource Enemy;
	[Export] public float SpawnChance;
	[Export] public float MinAmount;
	[Export] public float MaxAmount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
}
