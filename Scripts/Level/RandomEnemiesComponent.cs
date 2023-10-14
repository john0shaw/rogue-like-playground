using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

[Tool]
public partial class RandomEnemiesComponent : Node
{
	// Gameplay-Specific
	public static Vector2I SPAWNER_TILE = new Vector2I(0, 0);

	TileMap _spawnerTiles;
	PackedScene _enemyScene;
	Array<EnemySpawner> _enemySpawners;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Characters/Enemies/Enemy.tscn");
		_spawnerTiles = GetNode<TileMap>("SpawnLocations");
		_spawnerTiles.SetLayerEnabled(0, false);

		Node enemies = GetNode<Node>("Enemies");
		Array<Vector2I> spawnLocations = _spawnerTiles.GetUsedCellsById(0, 0, SPAWNER_TILE);
		Vector2 rootPosition = GetOwner<Node2D>().GlobalPosition;

		foreach (EnemySpawner enemySpawner in enemies.GetChildren())
		{
			for (int i = enemySpawner.MinAmount; i <= enemySpawner.MaxAmount; i++)
			{
				if (GD.Randf() < (enemySpawner.SpawnChance * GameState.GetLevelEnemyMultiplier()))
				{
                    EnemyController enemy = (EnemyController)_enemyScene.Instantiate();
                    enemy.EnemyResource = enemySpawner.Enemy;
                    enemy.Position = rootPosition + Room.GetTilePosition(spawnLocations.PickRandom());

                    AddChild(enemy);
                }
            }
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = new List<string>();
		if (GetNode<TileMap>("SpawnLocations") is not TileMap)
			warnings.Add("Must have TileMap named 'SpawnLocations'");
		if (GetNode<Node>("Enemies") is not Node)
			warnings.Add("Must have a Node named 'Enemies'");

		return warnings.ToArray();
	}

	public void _on_child_entered_tree(Node node)
	{
		if (Engine.IsEditorHint())
		{
			UpdateConfigurationWarnings();
		}
	}

	public void _on_child_exiting_tree(Node node)
	{
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
    }
}
