using Godot;
using Godot.Collections;
using System;

public partial class Room : TileMap
{
    const int BASE_LAYER = 0;
    const int DUNGEON_SOURCE = 1;
    const int TILE_SIZE = 16;
    const int ROOM_TILES = 40;

	[ExportGroup("Available Connections")]
	[Export] public bool HasNorthConnection { get; private set; }
	[Export] public bool HasEastConnection { get; private set; }
	[Export] public bool HasSouthConnection { get; private set; }
	[Export] public bool HasWestConnection { get; private set; }

	[ExportGroup("Used Connections")]
	[Export] public bool IsNorthConnection { get; private set; }
    [Export] public bool IsEastConnection { get; private set; }
    [Export] public bool IsSouthConnection { get; private set; }
    [Export] public bool IsWestConnection { get; private set; }

    [ExportGroup("Features")]
    [Export] public Array<EnemySpawner> Enemies = new Array<EnemySpawner>();

    public Vector2 MapPosition;

    public Connection AvailableConnections = new Connection();
    public Connection UsedConnections = new Connection();

    Dictionary<string, int> _tileLayerMap = new Dictionary<string, int>();
    RandomDungeonGenerator _dungeon;
    RandomNumberGenerator _rng;
    Array<Vector2I> _baseTiles;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        AvailableConnections.North = HasNorthConnection;
        AvailableConnections.East = HasEastConnection;
        AvailableConnections.South = HasSouthConnection;
        AvailableConnections.West = HasWestConnection;

        UsedConnections.North = IsNorthConnection;
        UsedConnections.East = IsEastConnection;
        UsedConnections.South = IsSouthConnection;
        UsedConnections.West = IsWestConnection;

		for (int i = 0; i < GetLayersCount(); i++)
		{
			_tileLayerMap.Add(GetLayerName(i), i);
		}

        _dungeon = RandomDungeonGenerator.Instance;
    }

    public void SetConnection(Direction direction, bool state)
    {
        UsedConnections.Set(direction, state);
        switch(direction)
        {
            case Direction.North:
                SetLayerEnabled(_tileLayerMap["NorthExit"], !state);
                break;
            case Direction.East:
                SetLayerEnabled(_tileLayerMap["EastExit"], !state);
                break;
            case Direction.South:
                SetLayerEnabled(_tileLayerMap["SouthExit"], !state);
                break;
            case Direction.West:
                SetLayerEnabled(_tileLayerMap["WestExit"], !state);
                break;
        }
    }

    private Vector2 CalculateGlobalPosition(Vector2 mapPosition)
    {
        return new Vector2(mapPosition.X * TILE_SIZE * ROOM_TILES, mapPosition.Y * TILE_SIZE * ROOM_TILES);
    }

    private Vector2 CalculateTilePosition(Vector2 tilePosition)
    {
        return new Vector2(
            (tilePosition.X * TILE_SIZE) + (TILE_SIZE / 2), 
            (tilePosition.Y * TILE_SIZE) + (TILE_SIZE / 2)
        );
    }

    public void Setup(Vector2 mapPosition, RandomNumberGenerator rng)
    {
        _rng = rng;
        GlobalPosition = CalculateGlobalPosition(mapPosition);
        MapPosition = mapPosition;

        _baseTiles = GetUsedCellsById(BASE_LAYER, DUNGEON_SOURCE, atlasCoords: _dungeon.BaseGroundTile);
        if (_tileLayerMap.ContainsKey("EnemySpawn"))
            SetLayerModulate(_tileLayerMap["EnemySpawn"], new Color(1f, 1f, 1f, 0f));

        RandomizeGround();
        RandomizeWalls();
        SpawnEnemies();
    }

    public void RandomizeGround()
    {
        foreach(Vector2I cellPosition in _baseTiles)
        {
            if (_rng.Randf() < 0.1f)
            {
                SetCell(BASE_LAYER, cellPosition, DUNGEON_SOURCE, _dungeon.AlternateGroundTiles.PickRandom());
            }
        }
    }

    public void RandomizeWalls()
    {
        for (int layer = 0; layer < GetLayersCount(); layer++)
        {
            foreach (Vector2I cellPosition in GetUsedCellsById(layer, DUNGEON_SOURCE, _dungeon.BaseWallTile))
            {
                if (_rng.Randf() < 0.1f)
                {
                    SetCell(layer, cellPosition, DUNGEON_SOURCE, _dungeon.AlternateWallTiles.PickRandom());
                }
            }
        }
    }

    public void SpawnEnemies()
    {
        if (Enemies.Count == 0)
            return;

        Array<Vector2I> spawnTiles = GetUsedCellsById(_tileLayerMap["EnemySpawn"], DUNGEON_SOURCE, _dungeon.EnemySpawnTile);
        if (spawnTiles.Count == 0)
            return;

        foreach (EnemySpawner spawner in Enemies)
        {
            for (int i = 0; i < spawner.MaxSpawn; i++)
            {
                if (_rng.Randf() < spawner.SpawnChance)
                {
                    EnemyController enemy = (EnemyController)spawner.Scene.Instantiate();
                    AddChild(enemy);
                    enemy.Position = CalculateTilePosition(spawnTiles.PickRandom());
                }
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
