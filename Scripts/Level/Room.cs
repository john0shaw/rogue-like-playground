using Godot;
using Godot.Collections;
using System;

public partial class Room : TileMap
{
    public const int TILE_SIZE = 16;
    public const int BASE_LAYER = 0;
    public const int ROOM_SOURCE = 1;
    public const int ROOM_TILES = 27;

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

    public static Vector2 GetTilePosition(Vector2 tilePosition)
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

        _baseTiles = GetUsedCellsById(BASE_LAYER, ROOM_SOURCE, atlasCoords: _dungeon.BaseGroundTile);

        if (_tileLayerMap.ContainsKey("EnemySpawn"))
            SetLayerModulate(_tileLayerMap["EnemySpawn"], new Color(1f, 1f, 1f, 0f));

        RandomizeGround();
    }

    public void RandomizeGround()
    {
        foreach(Vector2I cellPosition in _baseTiles)
        {
            if (_rng.Randf() < 0.1f)
            {
                SetCell(BASE_LAYER, cellPosition, ROOM_SOURCE, _dungeon.AlternateGroundTiles.PickRandom());
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
