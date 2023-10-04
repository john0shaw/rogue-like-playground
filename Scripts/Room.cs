using Godot;
using Godot.Collections;
using System;

public partial class Room : TileMap
{
	[ExportGroup("AvailableConnections")]
	[Export] public bool HasNorthConnection { get; private set; }
	[Export] public bool HasEastConnection { get; private set; }
	[Export] public bool HasSouthConnection { get; private set; }
	[Export] public bool HasWestConnection { get; private set; }

	[ExportGroup("UsedConnections")]
	[Export] public bool IsNorthConnection { get; private set; }
    [Export] public bool IsEastConnection { get; private set; }
    [Export] public bool IsSouthConnection { get; private set; }
    [Export] public bool IsWestConnection { get; private set; }

    public Vector2 MapPosition;

    public Connection AvailableConnections = new Connection();
    public Connection UsedConnections = new Connection();

    private Dictionary<string, int> _tileLayerMap = new Dictionary<string, int>();

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
