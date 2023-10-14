using Godot;
using Godot.Collections;
using System;

public partial class Room : TileMap
{
    public const int TILE_SIZE = 16;
    public const int BASE_LAYER = 0;
    public const int ROOM_SOURCE = 0;
    public const int ROOM_TILES = 27;

    [ExportGroup("Connections")]
	[ExportSubgroup("Available")]
	[Export] public bool HasNorthConnection { get; private set; }
	[Export] public bool HasEastConnection { get; private set; }
	[Export] public bool HasSouthConnection { get; private set; }
	[Export] public bool HasWestConnection { get; private set; }

	[ExportSubgroup("Used")]
	[Export] public bool IsNorthConnection { get; private set; }
    [Export] public bool IsEastConnection { get; private set; }
    [Export] public bool IsSouthConnection { get; private set; }
    [Export] public bool IsWestConnection { get; private set; }

    [ExportGroup("Tile Overrides")]
    [Export] public RandomTileReplacementResource RandomFloorTiles;

    public Vector2 MapPosition;

    public Connection AvailableConnections = new Connection();
    public Connection UsedConnections = new Connection();

    Dictionary<string, int> _tileLayerMap = new Dictionary<string, int>();

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

        RandomizeGround();
    }

    /// <summary>
    /// Enable or disable a connection to another room and set the layer state for display
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="state"></param>
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

    /// <summary>
    /// Get a tiles relative position within the node based on a given x,y cell position
    /// </summary>
    /// <param name="tilePosition">Position of the tile in the room starting at (0,0)</param>
    /// <returns>Relative positon within the node</returns>
    public static Vector2 GetTilePosition(Vector2 tilePosition)
    {
        return new Vector2(
            (tilePosition.X * TILE_SIZE) + (TILE_SIZE / 2),
            (tilePosition.Y * TILE_SIZE) + (TILE_SIZE / 2)
        );
    }

    public void RandomizeGround()
    {
        if (RandomFloorTiles is RandomTileReplacementResource)
        {
            foreach (Vector2I baseTilePosition in GetUsedCellsById(BASE_LAYER, ROOM_SOURCE, RandomFloorTiles.BaseTile))
            {
                if (GD.Randf() < RandomFloorTiles.ReplacementChance)
                {
                    SetCell(BASE_LAYER, baseTilePosition, ROOM_SOURCE, RandomFloorTiles.ReplacementTiles.PickRandom());
                }
            }
        }
    }
}
