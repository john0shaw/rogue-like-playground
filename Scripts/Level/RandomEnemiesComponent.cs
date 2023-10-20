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
	Vector2 _rootPosition;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Characters/Enemies/BasicEnemy.tscn");
		_spawnerTiles = GetNode<TileMap>("SpawnLocations");
		_spawnerTiles.SetLayerEnabled(0, false);

		Node enemies = GetNode<Node>("Enemies");
		Array<Vector2I> spawnLocations = _spawnerTiles.GetUsedCellsById(0, 0, SPAWNER_TILE);
		_rootPosition = GetOwner<Node2D>().GlobalPosition;

		foreach (EnemySpawner enemySpawner in enemies.GetChildren())
		{
			if (enemySpawner.Enemy.MinSpawnLevel > GameState.Level)
				continue;

			EnemyResource enemyResource = (EnemyResource)enemySpawner.Enemy.Duplicate();

            enemyResource.MaxHealth *= GameState.EnemyHealthMultiplier();
            enemyResource.AttackDamage *= GameState.EnemyDamageMultiplier();

            for (int i = 0; i <= enemySpawner.MaxAmount; i++)
			{
				if (i <= (enemySpawner.MinAmount * GameState.EnemySpawnMultiplier()) || GD.Randf() < (enemySpawner.SpawnChance * GameState.EnemySpawnMultiplier()))
				{
                    SpawnEnemy(enemyResource, spawnLocations.PickRandom());
                }
            }
		}
	}

	void SpawnEnemy(EnemyResource enemyResource, Vector2I tilePosition)
	{
        EnemyController enemy = (EnemyController)_enemyScene.Instantiate();

		enemy.EnemyResource = enemyResource;
        enemy.Position = _rootPosition + Room.GetTilePosition(tilePosition);

        AddChild(enemy);
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
